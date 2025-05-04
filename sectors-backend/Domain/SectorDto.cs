﻿namespace Domain;

public class SectorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public List<SectorDto>? Children { get; set; }
}