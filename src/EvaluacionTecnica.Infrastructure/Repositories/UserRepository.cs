using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;
using EvaluacionTecnica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionTecnica.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Include(u => u.Role)
            .ToListAsync();
    }

    public async Task<User?> GetByUsuarioNombreAsync(string usuarioNombre)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Usuario_Nombre == usuarioNombre);
    }

    public async Task<User?> GetByCedulaAsync(string cedula)
    {
        return await _dbSet
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Cedula == cedula);
    }

    public async Task<bool> ExistsByCedulaAsync(string cedula)
    {
        return await _dbSet.AnyAsync(u => u.Cedula == cedula);
    }

    public async Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre)
    {
        return await _dbSet.AnyAsync(u => u.Usuario_Nombre == usuarioNombre);
    }

    public async Task<IEnumerable<User>> GetByRoleIdAsync(int roleId)
    {
        return await _dbSet
            .Include(u => u.Role)
            .Where(u => u.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetFilteredAsync(
        string? searchText = null,
        int? roleId = null,
        string? cedula = null,
        DateTime? fechaNacimientoDesde = null,
        DateTime? fechaNacimientoHasta = null,
        string? orderBy = null)
    {
        var query = _dbSet.Include(u => u.Role).AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(u =>
                u.Nombre.Contains(searchText) ||
                u.Apellido.Contains(searchText) ||
                u.Usuario_Nombre.Contains(searchText));
        }

        if (roleId.HasValue)
        {
            query = query.Where(u => u.RoleId == roleId.Value);
        }

        if (!string.IsNullOrWhiteSpace(cedula))
        {
            query = query.Where(u => u.Cedula.Contains(cedula));
        }

        if (fechaNacimientoDesde.HasValue)
        {
            query = query.Where(u => u.Fecha_Nacimiento >= fechaNacimientoDesde.Value);
        }

        if (fechaNacimientoHasta.HasValue)
        {
            query = query.Where(u => u.Fecha_Nacimiento <= fechaNacimientoHasta.Value);
        }

        query = orderBy?.ToLower() switch
        {
            "fecha_nacimiento" => query.OrderBy(u => u.Fecha_Nacimiento),
            "fecha_nacimiento_desc" => query.OrderByDescending(u => u.Fecha_Nacimiento),
            "nombre" => query.OrderBy(u => u.Nombre),
            "apellido" => query.OrderBy(u => u.Apellido),
            _ => query.OrderBy(u => u.Id)
        };

        return await query.ToListAsync();
    }
}
