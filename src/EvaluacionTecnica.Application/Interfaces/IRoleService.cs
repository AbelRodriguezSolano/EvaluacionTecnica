using EvaluacionTecnica.Application.DTOs;

namespace EvaluacionTecnica.Application.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(CreateRoleDto createRoleDto, string currentUser);
    Task<bool> UpdateAsync(UpdateRoleDto updateRoleDto, string currentUser);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(string nombre);
    Task<bool> ExistsAsync(string nombre, int excludeId);
}
