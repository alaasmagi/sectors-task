namespace WebApp.Models;

public class PersonModel
{
    public int? PersonId { get; set; }
    public required string FullName { get; set; }
    public required int SectorId { get; set; }
    public required bool Agreement { get; set; }
    public required string Origin { get; set; }
}