using EvaluacionTecnica.Application.DTOs;

namespace EvaluacionTecnica.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto?> ValidateCredentialsAsync(string usuarioNombre, string contraseña);
}
