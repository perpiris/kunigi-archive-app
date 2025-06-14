namespace KunigiArchive.Contracts.Team;

public record TeamEditRequest(
    string Slug, 
    bool IsActive, 
    bool IsArchived, 
    short? YearFounded, 
    string? Description,
    string? FacebookLink,
    string? InstagramLink,
    string? YoutubeLink,
    string? WebsiteLink);