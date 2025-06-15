using KunigiArchive.Application.Common;
using Microsoft.AspNetCore.Http;

namespace KunigiArchive.Application.Services;

public interface IFileService
{
    Task<ServiceResult> CreateMediaFolderAsync(string folderName);
    
    Task<ServiceResult<string>> SaveFileAsync(IFormFile file, string folderPath);
    
    ServiceResult DeleteFile(string? filePath);
}