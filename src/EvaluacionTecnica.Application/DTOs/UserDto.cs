namespace EvaluacionTecnica.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Usuario_Nombre { get; set; } = string.Empty;
    public DateTime Fecha_Nacimiento { get; set; }
    public string Usuario_Transaccion { get; set; } = string.Empty;
    public DateTime Fecha_Transaccion { get; set; }
}
