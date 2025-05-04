using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Base.Domain;

namespace Domain;

public class SectorEntity : BaseEntity
{
    [Required]
    public string Name { get; set; } = default!;

    [JsonIgnore] 
    public SectorEntity? Parent { get; set; }
    public int? ParentId { get; set; }

    public List<SectorEntity>? Children { get; set; } = [];
}