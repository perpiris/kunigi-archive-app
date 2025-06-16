using System.Security.Cryptography;
using KunigiArchive.Application.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<FileService> _logger;
    private const string MediaFolderName = "media";
    
    public FileService(
        IWebHostEnvironment webHostEnvironment, 
        ILogger<FileService> logger)
    {
        ArgumentNullException.ThrowIfNull(webHostEnvironment);
        ArgumentNullException.ThrowIfNull(logger);
        
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
    
    public Task<ServiceResult> CreateMediaFolderAsync(string folderName)
    {
        try
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, MediaFolderName, folderName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the media folder {FolderName}", folderName);
            return Task.FromResult(ServiceResult.Failure("Failed to create media folder."));
        }
        
        return Task.FromResult(ServiceResult.Success());
    }
    
    public async Task<ServiceResult<string>> SaveFileAsync(IFormFile file, string folderPath)
    {
        try
        {
            var fullFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, MediaFolderName, folderPath);
        
            if (!Directory.Exists(fullFolderPath))
            {
                Directory.CreateDirectory(fullFolderPath);
            }

            var extension = Path.GetExtension(file.FileName);
            string fileName;
            string fullFilePath;

            do
            {
                var buffer = new byte[6];
                RandomNumberGenerator.Fill(buffer);
                var randomUrlSafeString = Convert.ToBase64String(buffer)
                    .Replace("+", "-")
                    .Replace("/", "_");

                fileName = $"{randomUrlSafeString}{extension}";
                fullFilePath = Path.Combine(fullFolderPath, fileName);
            } 
            while (File.Exists(fullFilePath));


            await using var stream = new FileStream(fullFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            var relativePath = Path.Combine(MediaFolderName, folderPath, fileName).Replace("\\", "/");
            return ServiceResult<string>.Success(relativePath);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving the file in folder {FolderPath}", folderPath);
            return ServiceResult<string>.Failure("An error occurred while saving the file.");
        }
    }

    public ServiceResult DeleteFile(string? filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return ServiceResult.Success();
        }

        try
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
            
            return ServiceResult.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting file at path {FilePath}", filePath);
            return ServiceResult.Failure("Error deleting file.");
        }
    }
}