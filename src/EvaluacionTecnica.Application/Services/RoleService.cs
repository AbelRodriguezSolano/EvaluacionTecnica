using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;

namespace EvaluacionTecnica.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();
        return roles.Select(r => new RoleDto
        {
            Id = r.Id,
            Nombre = r.Nombre,
            Usuario_Transaccion = r.Usuario_Transaccion,
            Fecha_Transaccion = r.Fecha_Transaccion
        });
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null) return null;

        return new RoleDto
        {
            Id = role.Id,
            Nombre = role.Nombre,
            Usuario_Transaccion = role.Usuario_Transaccion,
            Fecha_Transaccion = role.Fecha_Transaccion
        };
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto createRoleDto, string currentUser)
    {
        var role = new Role
        {
            Nombre = createRoleDto.Nombre,
            Usuario_Transaccion = currentUser,
            Fecha_Transaccion = DateTime.Now
        };

        var createdRole = await _roleRepository.AddAsync(role);

        return new RoleDto
        {
            Id = createdRole.Id,
            Nombre = createdRole.Nombre,
            Usuario_Transaccion = createdRole.Usuario_Transaccion,
            Fecha_Transaccion = createdRole.Fecha_Transaccion
        };
    }

    public async Task<bool> UpdateAsync(UpdateRoleDto updateRoleDto, string currentUser)
    {
        var role = await _roleRepository.GetByIdAsync(updateRoleDto.Id);
        if (role == null) return false;

        role.Nombre = updateRoleDto.Nombre;
        role.Usuario_Transaccion = currentUser;
        role.Fecha_Transaccion = DateTime.Now;

        await _roleRepository.UpdateAsync(role);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);
        if (role == null) return false;

        await _roleRepository.DeleteAsync(role);
        return true;
    }

    public async Task<bool> ExistsAsync(string nombre)
    {
        return await _roleRepository.ExistsAsync(nombre);
    }

    public async Task<bool> ExistsAsync(string nombre, int excludeId)
    {
        var role = await _roleRepository.GetByNombreAsync(nombre);
        return role != null && role.Id != excludeId;
    }
}
