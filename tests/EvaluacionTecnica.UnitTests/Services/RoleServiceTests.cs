using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Services;
using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;
using Moq;

namespace EvaluacionTecnica.UnitTests.Services;

public class RoleServiceTests
{
    private readonly Mock<IRoleRepository> _mockRoleRepository;
    private readonly RoleService _roleService;

    public RoleServiceTests()
    {
        _mockRoleRepository = new Mock<IRoleRepository>();
        _roleService = new RoleService(_mockRoleRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllRoles()
    {
        var roles = new List<Role>
        {
            new Role { Id = 1, Nombre = "ADMIN", Usuario_Transaccion = "SYSTEM", Fecha_Transaccion = DateTime.Now },
            new Role { Id = 2, Nombre = "USER", Usuario_Transaccion = "SYSTEM", Fecha_Transaccion = DateTime.Now }
        };

        _mockRoleRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(roles);

        var result = await _roleService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("ADMIN", result.First().Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsRole()
    {
        var role = new Role
        {
            Id = 1,
            Nombre = "ADMIN",
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(role);

        var result = await _roleService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("ADMIN", result.Nombre);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((Role?)null);

        var result = await _roleService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ReturnsCreatedRole()
    {
        var createRoleDto = new CreateRoleDto { Nombre = "NEW_ROLE" };
        var createdRole = new Role
        {
            Id = 3,
            Nombre = "NEW_ROLE",
            Usuario_Transaccion = "TEST_USER",
            Fecha_Transaccion = DateTime.Now
        };

        _mockRoleRepository.Setup(repo => repo.AddAsync(It.IsAny<Role>()))
            .ReturnsAsync(createdRole);

        var result = await _roleService.CreateAsync(createRoleDto, "TEST_USER");

        Assert.NotNull(result);
        Assert.Equal("NEW_ROLE", result.Nombre);
        Assert.Equal("TEST_USER", result.Usuario_Transaccion);
        _mockRoleRepository.Verify(repo => repo.AddAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ReturnsTrue()
    {
        var existingRole = new Role
        {
            Id = 1,
            Nombre = "OLD_NAME",
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };
        var updateRoleDto = new UpdateRoleDto { Id = 1, Nombre = "NEW_NAME" };

        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingRole);
        _mockRoleRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Role>()))
            .Returns(Task.CompletedTask);

        var result = await _roleService.UpdateAsync(updateRoleDto, "TEST_USER");

        Assert.True(result);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsFalse()
    {
        var updateRoleDto = new UpdateRoleDto { Id = 999, Nombre = "NEW_NAME" };

        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((Role?)null);

        var result = await _roleService.UpdateAsync(updateRoleDto, "TEST_USER");

        Assert.False(result);
        _mockRoleRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        var existingRole = new Role
        {
            Id = 1,
            Nombre = "ADMIN",
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingRole);
        _mockRoleRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Role>()))
            .Returns(Task.CompletedTask);

        var result = await _roleService.DeleteAsync(1);

        Assert.True(result);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Role>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        _mockRoleRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((Role?)null);

        var result = await _roleService.DeleteAsync(999);

        Assert.False(result);
        _mockRoleRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Role>()), Times.Never);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingName_ReturnsTrue()
    {
        _mockRoleRepository.Setup(repo => repo.ExistsAsync("ADMIN"))
            .ReturnsAsync(true);

        var result = await _roleService.ExistsAsync("ADMIN");

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingName_ReturnsFalse()
    {
        _mockRoleRepository.Setup(repo => repo.ExistsAsync("NONEXISTENT"))
            .ReturnsAsync(false);

        var result = await _roleService.ExistsAsync("NONEXISTENT");

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithExcludeId_WhenNameExistsInDifferentRole_ReturnsTrue()
    {
        var existingRole = new Role { Id = 2, Nombre = "ADMIN" };

        _mockRoleRepository.Setup(repo => repo.GetByNombreAsync("ADMIN"))
            .ReturnsAsync(existingRole);

        var result = await _roleService.ExistsAsync("ADMIN", 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsAsync_WithExcludeId_WhenNameExistsInSameRole_ReturnsFalse()
    {
        var existingRole = new Role { Id = 1, Nombre = "ADMIN" };

        _mockRoleRepository.Setup(repo => repo.GetByNombreAsync("ADMIN"))
            .ReturnsAsync(existingRole);

        var result = await _roleService.ExistsAsync("ADMIN", 1);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsAsync_WithExcludeId_WhenNameDoesNotExist_ReturnsFalse()
    {
        _mockRoleRepository.Setup(repo => repo.GetByNombreAsync("NONEXISTENT"))
            .ReturnsAsync((Role?)null);

        var result = await _roleService.ExistsAsync("NONEXISTENT", 1);

        Assert.False(result);
    }
}
