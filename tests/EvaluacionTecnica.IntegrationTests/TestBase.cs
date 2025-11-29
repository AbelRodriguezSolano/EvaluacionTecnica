using EvaluacionTecnica.Application.Interfaces;
using EvaluacionTecnica.Application.Services;
using EvaluacionTecnica.Domain.Interfaces;
using EvaluacionTecnica.Infrastructure.Data;
using EvaluacionTecnica.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EvaluacionTecnica.IntegrationTests;

public class TestBase : IDisposable
{
    protected ApplicationDbContext Context { get; }
    protected IRoleRepository RoleRepository { get; }
    protected IUserRepository UserRepository { get; }
    protected IRoleService RoleService { get; }
    protected IUserService UserService { get; }
    protected IPasswordHasher PasswordHasher { get; }

    public TestBase()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        Context = new ApplicationDbContext(options);

        RoleRepository = new RoleRepository(Context);
        UserRepository = new UserRepository(Context);
        PasswordHasher = new PasswordHasher();

        RoleService = new RoleService(RoleRepository);
        UserService = new UserService(UserRepository, PasswordHasher);
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
    }
}
