using EvaluacionTecnica.Domain.Entities;

namespace EvaluacionTecnica.Domain.Interfaces;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNombreAsync(string nombre);
    Task<bool> ExistsAsync(string nombre);
}
