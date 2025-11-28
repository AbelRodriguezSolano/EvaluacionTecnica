namespace EvaluacionTecnica.Application.DTOs;

public class RoleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Usuario_Transaccion { get; set; } = string.Empty;
    public DateTime Fecha_Transaccion { get; set; }
}
