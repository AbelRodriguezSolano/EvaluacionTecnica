using EvaluacionTecnica.Application.DTOs;

namespace EvaluacionTecnica.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto createUserDto, string currentUser);
    Task<bool> UpdateAsync(UpdateUserDto updateUserDto, string currentUser);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByCedulaAsync(string cedula);
    Task<bool> ExistsByCedulaAsync(string cedula, int excludeId);
    Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre);
    Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre, int excludeId);
    Task<IEnumerable<UserDto>> GetByRoleIdAsync(int roleId);
    Task<IEnumerable<UserDto>> GetFilteredAsync(UserFilterDto filter);
}
