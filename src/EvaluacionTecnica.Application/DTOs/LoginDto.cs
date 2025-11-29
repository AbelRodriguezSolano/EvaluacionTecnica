using System.ComponentModel.DataAnnotations;

namespace EvaluacionTecnica.Application.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    [Display(Name = "Usuario")]
    public string Usuario_Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es requerida")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Contraseña { get; set; } = string.Empty;
}
