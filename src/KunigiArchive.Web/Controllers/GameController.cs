using KunigiArchive.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

[Route("games")]
public class GameController : Controller
{
    private readonly IGameService  _gameService;

    public GameController(IGameService gameService)
    {
        ArgumentNullException.ThrowIfNull(gameService);
        
        _gameService = gameService;
    }
}