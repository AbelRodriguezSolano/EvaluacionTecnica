using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using EvaluacionTecnica.Application.Services;
using EvaluacionTecnica.Domain.Entities;
using EvaluacionTecnica.Domain.Interfaces;
using Moq;

namespace EvaluacionTecnica.UnitTests.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _userService = new UserService(_mockUserRepository.Object, _mockPasswordHasher.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Nombre = "John",
                Apellido = "Doe",
                Cedula = "123456",
                Usuario_Nombre = "jdoe",
                Contraseña = "hashed_password",
                Fecha_Nacimiento = new DateTime(1990, 1, 1),
                RoleId = 1,
                Role = new Role { Id = 1, Nombre = "ADMIN" },
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = DateTime.Now
            },
            new User
            {
                Id = 2,
                Nombre = "Jane",
                Apellido = "Smith",
                Cedula = "789012",
                Usuario_Nombre = "jsmith",
                Contraseña = "hashed_password",
                Fecha_Nacimiento = new DateTime(1995, 5, 15),
                RoleId = 2,
                Role = new Role { Id = 2, Nombre = "USER" },
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = DateTime.Now
            }
        };

        _mockUserRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(users);

        var result = await _userService.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("John", result.First().Nombre);
        Assert.Equal("ADMIN", result.First().RoleName);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsUser()
    {
        var user = new User
        {
            Id = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "hashed_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1),
            RoleId = 1,
            Role = new Role { Id = 1, Nombre = "ADMIN" },
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(user);

        var result = await _userService.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("John", result.Nombre);
        Assert.Equal("ADMIN", result.RoleName);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var result = await _userService.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_HashesPasswordAndReturnsCreatedUser()
    {
        var createUserDto = new CreateUserDto
        {
            RoleId = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "plain_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1)
        };

        var hashedPassword = "hashed_password_123";
        _mockPasswordHasher.Setup(h => h.HashPassword("plain_password"))
            .Returns(hashedPassword);

        var createdUser = new User
        {
            Id = 1,
            RoleId = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = hashedPassword,
            Fecha_Nacimiento = new DateTime(1990, 1, 1),
            Usuario_Transaccion = "TEST_USER",
            Fecha_Transaccion = DateTime.Now,
            Role = new Role { Id = 1, Nombre = "ADMIN" }
        };

        _mockUserRepository.Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(createdUser);
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(createdUser);

        var result = await _userService.CreateAsync(createUserDto, "TEST_USER");

        Assert.NotNull(result);
        Assert.Equal("John", result.Nombre);
        Assert.Equal("TEST_USER", result.Usuario_Transaccion);
        _mockPasswordHasher.Verify(h => h.HashPassword("plain_password"), Times.Once);
        _mockUserRepository.Verify(repo => repo.AddAsync(It.Is<User>(u => u.Contraseña == hashedPassword)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidDataAndPassword_HashesPasswordAndReturnsTrue()
    {
        var existingUser = new User
        {
            Id = 1,
            RoleId = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "old_hashed_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1),
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        var updateUserDto = new UpdateUserDto
        {
            Id = 1,
            RoleId = 1,
            Nombre = "John Updated",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "new_plain_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1)
        };

        var newHashedPassword = "new_hashed_password_123";
        _mockPasswordHasher.Setup(h => h.HashPassword("new_plain_password"))
            .Returns(newHashedPassword);

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingUser);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var result = await _userService.UpdateAsync(updateUserDto, "TEST_USER");

        Assert.True(result);
        _mockPasswordHasher.Verify(h => h.HashPassword("new_plain_password"), Times.Once);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u => u.Contraseña == newHashedPassword)), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithoutPassword_DoesNotHashPasswordAndReturnsTrue()
    {
        var existingUser = new User
        {
            Id = 1,
            RoleId = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "existing_hashed_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1),
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        var updateUserDto = new UpdateUserDto
        {
            Id = 1,
            RoleId = 1,
            Nombre = "John Updated",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = null,
            Fecha_Nacimiento = new DateTime(1990, 1, 1)
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingUser);
        _mockUserRepository.Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var result = await _userService.UpdateAsync(updateUserDto, "TEST_USER");

        Assert.True(result);
        _mockPasswordHasher.Verify(h => h.HashPassword(It.IsAny<string>()), Times.Never);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.Is<User>(u => u.Contraseña == "existing_hashed_password")), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithInvalidId_ReturnsFalse()
    {
        var updateUserDto = new UpdateUserDto
        {
            Id = 999,
            RoleId = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Fecha_Nacimiento = new DateTime(1990, 1, 1)
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var result = await _userService.UpdateAsync(updateUserDto, "TEST_USER");

        Assert.False(result);
        _mockUserRepository.Verify(repo => repo.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ReturnsTrue()
    {
        var existingUser = new User
        {
            Id = 1,
            Nombre = "John",
            Apellido = "Doe",
            Cedula = "123456",
            Usuario_Nombre = "jdoe",
            Contraseña = "hashed_password",
            Fecha_Nacimiento = new DateTime(1990, 1, 1),
            RoleId = 1,
            Usuario_Transaccion = "SYSTEM",
            Fecha_Transaccion = DateTime.Now
        };

        _mockUserRepository.Setup(repo => repo.GetByIdAsync(1))
            .ReturnsAsync(existingUser);
        _mockUserRepository.Setup(repo => repo.DeleteAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        var result = await _userService.DeleteAsync(1);

        Assert.True(result);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ReturnsFalse()
    {
        _mockUserRepository.Setup(repo => repo.GetByIdAsync(999))
            .ReturnsAsync((User?)null);

        var result = await _userService.DeleteAsync(999);

        Assert.False(result);
        _mockUserRepository.Verify(repo => repo.DeleteAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task ExistsByCedulaAsync_WithExistingCedula_ReturnsTrue()
    {
        _mockUserRepository.Setup(repo => repo.ExistsByCedulaAsync("123456"))
            .ReturnsAsync(true);

        var result = await _userService.ExistsByCedulaAsync("123456");

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByCedulaAsync_WithNonExistingCedula_ReturnsFalse()
    {
        _mockUserRepository.Setup(repo => repo.ExistsByCedulaAsync("999999"))
            .ReturnsAsync(false);

        var result = await _userService.ExistsByCedulaAsync("999999");

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByCedulaAsync_WithExcludeId_WhenCedulaExistsInDifferentUser_ReturnsTrue()
    {
        var existingUser = new User { Id = 2, Cedula = "123456" };

        _mockUserRepository.Setup(repo => repo.GetByCedulaAsync("123456"))
            .ReturnsAsync(existingUser);

        var result = await _userService.ExistsByCedulaAsync("123456", 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByCedulaAsync_WithExcludeId_WhenCedulaExistsInSameUser_ReturnsFalse()
    {
        var existingUser = new User { Id = 1, Cedula = "123456" };

        _mockUserRepository.Setup(repo => repo.GetByCedulaAsync("123456"))
            .ReturnsAsync(existingUser);

        var result = await _userService.ExistsByCedulaAsync("123456", 1);

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByUsuarioNombreAsync_WithExistingUsuarioNombre_ReturnsTrue()
    {
        _mockUserRepository.Setup(repo => repo.ExistsByUsuarioNombreAsync("jdoe"))
            .ReturnsAsync(true);

        var result = await _userService.ExistsByUsuarioNombreAsync("jdoe");

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByUsuarioNombreAsync_WithNonExistingUsuarioNombre_ReturnsFalse()
    {
        _mockUserRepository.Setup(repo => repo.ExistsByUsuarioNombreAsync("nonexistent"))
            .ReturnsAsync(false);

        var result = await _userService.ExistsByUsuarioNombreAsync("nonexistent");

        Assert.False(result);
    }

    [Fact]
    public async Task ExistsByUsuarioNombreAsync_WithExcludeId_WhenUsuarioNombreExistsInDifferentUser_ReturnsTrue()
    {
        var existingUser = new User { Id = 2, Usuario_Nombre = "jdoe" };

        _mockUserRepository.Setup(repo => repo.GetByUsuarioNombreAsync("jdoe"))
            .ReturnsAsync(existingUser);

        var result = await _userService.ExistsByUsuarioNombreAsync("jdoe", 1);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistsByUsuarioNombreAsync_WithExcludeId_WhenUsuarioNombreExistsInSameUser_ReturnsFalse()
    {
        var existingUser = new User { Id = 1, Usuario_Nombre = "jdoe" };

        _mockUserRepository.Setup(repo => repo.GetByUsuarioNombreAsync("jdoe"))
            .ReturnsAsync(existingUser);

        var result = await _userService.ExistsByUsuarioNombreAsync("jdoe", 1);

        Assert.False(result);
    }

    [Fact]
    public async Task GetByRoleIdAsync_ReturnsUsersWithSpecificRole()
    {
        var users = new List<User>
        {
            new User
            {
                Id = 1,
                Nombre = "John",
                Apellido = "Doe",
                Cedula = "123456",
                Usuario_Nombre = "jdoe",
                Contraseña = "hashed_password",
                Fecha_Nacimiento = new DateTime(1990, 1, 1),
                RoleId = 1,
                Role = new Role { Id = 1, Nombre = "ADMIN" },
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = DateTime.Now
            },
            new User
            {
                Id = 2,
                Nombre = "Jane",
                Apellido = "Smith",
                Cedula = "789012",
                Usuario_Nombre = "jsmith",
                Contraseña = "hashed_password",
                Fecha_Nacimiento = new DateTime(1995, 5, 15),
                RoleId = 1,
                Role = new Role { Id = 1, Nombre = "ADMIN" },
                Usuario_Transaccion = "SYSTEM",
                Fecha_Transaccion = DateTime.Now
            }
        };

        _mockUserRepository.Setup(repo => repo.GetByRoleIdAsync(1))
            .ReturnsAsync(users);

        var result = await _userService.GetByRoleIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.All(result, u => Assert.Equal("ADMIN", u.RoleName));
    }
}
