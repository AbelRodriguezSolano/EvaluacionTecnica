using EvaluacionTecnica.Domain.Entities;

namespace EvaluacionTecnica.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUsuarioNombreAsync(string usuarioNombre);
    Task<User?> GetByCedulaAsync(string cedula);
    Task<bool> ExistsByCedulaAsync(string cedula);
    Task<bool> ExistsByUsuarioNombreAsync(string usuarioNombre);
    Task<IEnumerable<User>> GetByRoleIdAsync(int roleId);
}
