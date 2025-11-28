using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;
using EvaluacionTecnica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionTecnica.Infrastructure.Repositories;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByNombreAsync(string nombre)
    {
        return await _dbSet.FirstOrDefaultAsync(r => r.Nombre == nombre);
    }

    public async Task<bool> ExistsAsync(string nombre)
    {
        return await _dbSet.AnyAsync(r => r.Nombre == nombre);
    }
}
