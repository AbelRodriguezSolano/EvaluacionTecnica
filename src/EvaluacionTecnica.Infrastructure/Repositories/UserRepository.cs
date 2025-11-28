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
}
