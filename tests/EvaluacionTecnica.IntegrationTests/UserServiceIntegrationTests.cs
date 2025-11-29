using EvaluacionTecnica.Application.DTOs;

namespace EvaluacionTecnica.IntegrationTests;

public class UserServiceIntegrationTests : TestBase
{
    private async Task<int> CreateTestRoleAsync()
    {
        var roleDto = new CreateRoleDto { Nombre = "ADMIN" };
        var role = await RoleService.CreateAsync(roleDto, "SYSTEM");
        return role.Id;
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateUser_WhenValidData()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Juan",
            Apellido = "Perez",
            Cedula = "12345678901",
            Usuario_Nombre = "jperez",
            Contraseña = "password123",
            Fecha_Nacimiento = new DateTime(1990, 1, 1)
        };

        var result = await UserService.CreateAsync(createUserDto, "SYSTEM");

        Assert.NotNull(result);
        Assert.Equal("Juan", result.Nombre);
        Assert.Equal("Perez", result.Apellido);
        Assert.Equal("12345678901", result.Cedula);
        Assert.Equal("jperez", result.Usuario_Nombre);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        var roleId = await CreateTestRoleAsync();
        var user1 = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Ana",
            Apellido = "Garcia",
            Cedula = "11111111111",
            Usuario_Nombre = "agarcia",
            Contraseña = "pass123",
            Fecha_Nacimiento = new DateTime(1995, 5, 15)
        };
        var user2 = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Luis",
            Apellido = "Martinez",
            Cedula = "22222222222",
            Usuario_Nombre = "lmartinez",
            Contraseña = "pass456",
            Fecha_Nacimiento = new DateTime(1988, 8, 20)
        };

        await UserService.CreateAsync(user1, "SYSTEM");
        await UserService.CreateAsync(user2, "SYSTEM");

        var users = await UserService.GetAllAsync();

        Assert.NotNull(users);
        Assert.Equal(2, users.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Maria",
            Apellido = "Lopez",
            Cedula = "33333333333",
            Usuario_Nombre = "mlopez",
            Contraseña = "secure123",
            Fecha_Nacimiento = new DateTime(1992, 3, 10)
        };
        var created = await UserService.CreateAsync(createUserDto, "SYSTEM");

        var result = await UserService.GetByIdAsync(created.Id);

        Assert.NotNull(result);
        Assert.Equal("Maria", result.Nombre);
        Assert.Equal("Lopez", result.Apellido);
        Assert.Equal(created.Id, result.Id);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_WhenValidData()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Carlos",
            Apellido = "Diaz",
            Cedula = "44444444444",
            Usuario_Nombre = "cdiaz",
            Contraseña = "mypass123",
            Fecha_Nacimiento = new DateTime(1985, 7, 25)
        };
        var created = await UserService.CreateAsync(createUserDto, "SYSTEM");

        var updateUserDto = new UpdateUserDto
        {
            Id = created.Id,
            RoleId = roleId,
            Nombre = "Carlos Alberto",
            Apellido = "Diaz Ruiz",
            Cedula = "44444444444",
            Usuario_Nombre = "cdiaz",
            Fecha_Nacimiento = new DateTime(1985, 7, 25)
        };

        var result = await UserService.UpdateAsync(updateUserDto, "SYSTEM");

        Assert.True(result);
        var updated = await UserService.GetByIdAsync(created.Id);
        Assert.Equal("Carlos Alberto", updated?.Nombre);
        Assert.Equal("Diaz Ruiz", updated?.Apellido);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Pedro",
            Apellido = "Sanchez",
            Cedula = "55555555555",
            Usuario_Nombre = "psanchez",
            Contraseña = "temp123",
            Fecha_Nacimiento = new DateTime(1993, 12, 5)
        };
        var created = await UserService.CreateAsync(createUserDto, "SYSTEM");

        var result = await UserService.DeleteAsync(created.Id);

        Assert.True(result);
        var deleted = await UserService.GetByIdAsync(created.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task ExistsByCedulaAsync_ShouldReturnTrue_WhenUserWithCedulaExists()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Sofia",
            Apellido = "Hernandez",
            Cedula = "66666666666",
            Usuario_Nombre = "shernandez",
            Contraseña = "pass789",
            Fecha_Nacimiento = new DateTime(1991, 4, 18)
        };
        await UserService.CreateAsync(createUserDto, "SYSTEM");

        var exists = await UserService.ExistsByCedulaAsync("66666666666");

        Assert.True(exists);
    }

    [Fact]
    public async Task ExistsByUsuarioNombreAsync_ShouldReturnTrue_WhenUserWithUsernameExists()
    {
        var roleId = await CreateTestRoleAsync();
        var createUserDto = new CreateUserDto
        {
            RoleId = roleId,
            Nombre = "Roberto",
            Apellido = "Torres",
            Cedula = "77777777777",
            Usuario_Nombre = "rtorres",
            Contraseña = "mypassword",
            Fecha_Nacimiento = new DateTime(1989, 9, 30)
        };
        await UserService.CreateAsync(createUserDto, "SYSTEM");

        var exists = await UserService.ExistsByUsuarioNombreAsync("rtorres");

        Assert.True(exists);
    }
}
