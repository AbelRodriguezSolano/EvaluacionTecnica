using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;

namespace EvaluacionTecnica.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => MapToDto(u));
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        return MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto, string currentUser)
    {
        var user = new User
        {
            RoleId = createUserDto.RoleId,
            Nombre = createUserDto.Nombre,
            Apellido = createUserDto.Apellido,
            Cedula = createUserDto.Cedula,
            Usuario_Nombre = createUserDto.Usuario_Nombre,
            Contraseña = _passwordHasher.HashPassword(createUserDto.Contraseña),
            Fecha_Nacimiento = createUserDto.Fecha_Nacimiento,
            Usuario_Transaccion = currentUser,
            Fecha_Transaccion = DateTime.Now
        };

        var createdUser = await _userRepository.AddAsync(user);
        var result = await _userRepository.GetByIdAsync(createdUser.Id);

        return MapToDto(result!);
    }

    public async Task<bool> UpdateAsync(UpdateUserDto updateUserDto, string currentUser)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null) return false;

        user.RoleId = updateUserDto.RoleId;
        user.Nombre = updateUserDto.Nombre;
        user.Apellido = updateUserDto.Apellido;
        user.Cedula = updateUserDto.Cedula;
        user.Usuario_Nombre = updateUserDto.Usuario_Nombre;
        user.Fecha_Nacimiento = updateUserDto.Fecha_Nacimiento;
        user.Usuario_Transaccion = currentUser;
        user.Fecha_Transaccion = DateTime.Now;

        if (!string.IsNullOrEmpty(updateUserDto.Contraseña))
        {
            user.Contraseña = _passwordHasher.HashPassword(updateUserDto.Contraseña);
        }

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteAsync(user);
        return true;
    }

    public async Task<bool> ExistsByCedulaAsync(string cedula)
    {
        return await _userRepository.ExistsByCedulaAsync(cedula);
    }

    public async Task<bool> ExistsByCedulaAsync(string cedula, int excludeId)
    {
        var user = await _userRepository.GetByCedulaAsync(cedula);
        return user != null && user.Id != excludeId;
    }

    public async Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre)
    {
        return await _userRepository.ExistsByUsuarioNombreAsync(usuarioNombre);
    }

    public async Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre, int excludeId)
    {
        var user = await _userRepository.GetByUsuarioNombreAsync(usuarioNombre);
        return user != null && user.Id != excludeId;
    }

    public async Task<IEnumerable<UserDto>> GetByRoleIdAsync(int roleId)
    {
        var users = await _userRepository.GetByRoleIdAsync(roleId);
        return users.Select(u => MapToDto(u));
    }

    private UserDto MapToDto(User user)
    {
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
