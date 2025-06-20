using System.ComponentModel.DataAnnotations;
using KunigiArchive.Web.ViewModels.UserManagement;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KunigiArchive.Web.ViewModels.Team;

public class TeamManagerEditViewModel
{
    public long TeamId { get; set; }
    
    public required string TeamName { get; set; }
    
    public required string Slug { get; set; }
    
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    public long SelectedUserId { get; set; }
    
    public List<UserDetailsViewModel> CurrentManagers { get; set; } = new();
    
    public SelectList? AvailableUsers { get; set; } 
}