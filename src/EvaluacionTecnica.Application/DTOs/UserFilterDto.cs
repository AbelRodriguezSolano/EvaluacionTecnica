namespace EvaluacionTecnica.Application.DTOs;

public class UserFilterDto
{
    public string? SearchText { get; set; }
    public int? RoleId { get; set; }
    public string? Cedula { get; set; }
    public DateTime? FechaNacimientoDesde { get; set; }
    public DateTime? FechaNacimientoHasta { get; set; }
    public string? OrderBy { get; set; }
}
