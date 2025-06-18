using System.ComponentModel.DataAnnotations;

namespace Domain;

public class PersonDto(PersonEntity person)
{
    public Guid ExternalId { get; set; } = person.ExternalId;
    public string FullName { get; set; } = person.FullName;
    public SectorEntity? Sector { get; set; } = person.Sector;
    public int SectorId { get; set; } = person.SectorId;
    public bool Agreement { get; set; } = person.Agreement;
}
