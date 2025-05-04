using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain;

namespace Domain;

public class  PersonEntity : BaseEntity
{
    [Required] 
    [MaxLength(128)]
    public string FullName { get; set; } = default!;
    
    public SectorEntity? Sector { get; set; }
    public int? SectorId { get; set; }

    [Required]
    public bool Agreement { get; set; }
}