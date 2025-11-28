using System.ComponentModel.DataAnnotations;

namespace EvaluacionTecnica.Application.DTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "El rol es requerido")]
    public int RoleId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es requerido")]
    [StringLength(100, ErrorMessage = "El apellido no puede exceder 100 caracteres")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "La cédula es requerida")]
    [StringLength(50, ErrorMessage = "La cédula no puede exceder 50 caracteres")]
    public string Cedula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [StringLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
    public string Usuario_Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "La contraseña debe tener entre 3 y 255 caracteres")]
    public string Contraseña { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    [DataType(DataType.Date)]
    public DateTime Fecha_Nacimiento { get; set; }
}
