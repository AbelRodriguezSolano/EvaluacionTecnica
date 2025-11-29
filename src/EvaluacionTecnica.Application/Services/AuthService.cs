using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using EvaluacionTecnica.Domain.Interfaces;

namespace EvaluacionTecnica.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> ValidateCredentialsAsync(string usuarioNombre, string contraseña)
    {
        var user = await _userRepository.GetByUsuarioNombreAsync(usuarioNombre);

        if (user == null)
        {
            return null;
        }

        bool isValidPassword = _passwordHasher.VerifyPassword(contraseña, user.Contraseña);

        if (!isValidPassword)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            RoleId = user.RoleId,
            RoleName = user.Role?.Nombre ?? string.Empty,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Cedula = user.Cedula,
            Usuario_Nombre = user.Usuario_Nombre,
            Fecha_Nacimiento = user.Fecha_Nacimiento,
            Usuario_Transaccion = user.Usuario_Transaccion,
            Fecha_Transaccion = user.Fecha_Transaccion
        };
    }
}
