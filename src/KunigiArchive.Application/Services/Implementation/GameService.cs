using KunigiArchive.Application.Data;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class GameService : IGameService
{
    private readonly DataContext _context;
    private readonly ILogger<TeamService> _logger;
    private readonly IFileService _fileService;

    public GameService(
        DataContext context, 
        ILogger<TeamService> logger, 
        IFileService fileService)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(fileService);
        
        _context = context;
        _logger = logger;
        _fileService = fileService;
    }
}