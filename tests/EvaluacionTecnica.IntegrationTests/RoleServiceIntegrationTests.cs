using EvaluacionTecnica.Application.DTOs;

namespace EvaluacionTecnica.IntegrationTests;

public class RoleServiceIntegrationTests : TestBase
{
    [Fact]
    public async Task CreateAsync_ShouldCreateRole_WhenValidData()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "TESTER" };

        var result = await RoleService.CreateAsync(createRoleDto, "SYSTEM");

        Assert.NotNull(result);
        Assert.Equal("TESTER", result.Nombre);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRoles()
    {
        var role1 = new CreateRoleDto { Nombre = "ADMIN" };
        var role2 = new CreateRoleDto { Nombre = "USER" };
        await RoleService.CreateAsync(role1, "SYSTEM");
        await RoleService.CreateAsync(role2, "SYSTEM");

        var roles = await RoleService.GetAllAsync();

        Assert.NotNull(roles);
        Assert.Equal(2, roles.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnRole_WhenRoleExists()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "DEVELOPER" };
        var created = await RoleService.CreateAsync(createRoleDto, "SYSTEM");

        var result = await RoleService.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("DEVELOPER", result.Nombre);
        Assert.Equal(created.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateRole_WhenValidData()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "MANAGER" };
        var created = await RoleService.CreateAsync(createRoleDto, "SYSTEM");

        var updateRoleDto = new UpdateRoleDto
        {
            Id = created.Id,
            Nombre = "SENIOR_MANAGER"
        };

        var result = await RoleService.UpdateAsync(updateRoleDto, "SYSTEM");

        Assert.True(result);
        var updated = await RoleService.GetByIdAsync(created.Id);
        Assert.Equal("SENIOR_MANAGER", updated?.Nombre);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteRole_WhenRoleExists()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "TEMP_ROLE" };
        var created = await RoleService.CreateAsync(createRoleDto, "SYSTEM");

        var result = await RoleService.DeleteAsync(created.Id);

        Assert.True(result);
        var deleted = await RoleService.GetByIdAsync(created.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task ExistsAsync_ShouldReturnTrue_WhenRoleExists()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "EXISTING_ROLE" };
        await RoleService.CreateAsync(createRoleDto, "SYSTEM");

        var exists = await RoleService.ExistsAsync("EXISTING_ROLE");

        Assert.True(exists);
    }
}
