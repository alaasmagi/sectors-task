namespace WebApp.Models;

public class PersonModel
{
    public Guid? ExternalId { get; set; }
    public required string FullName { get; set; }
    public required int SectorId { get; set; }
    public required bool Agreement { get; set; }
    public required string Origin { get; set; }
}