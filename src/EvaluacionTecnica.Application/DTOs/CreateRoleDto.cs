using System.ComponentModel.DataAnnotations;

namespace EvaluacionTecnica.Application.DTOs;

public class CreateRoleDto
{
    [Required(ErrorMessage = "El nombre del rol es requerido")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string Nombre { get; set; } = string.Empty;
}
