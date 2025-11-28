namespace EvaluacionTecnica.Domain.Entities;

public class User : BaseEntity
{
    public int RoleId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Usuario_Nombre { get; set; } = string.Empty;
    public string Contraseña { get; set; } = string.Empty;
    public DateTime Fecha_Nacimiento { get; set; }
    public Role Role { get; set; } = null!;
}
