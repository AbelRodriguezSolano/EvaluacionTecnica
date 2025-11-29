using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EvaluacionTecnica.Web.Controllers;

[Authorize]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UsersController(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateRolesDropdown();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRolesDropdown();
            return View(createUserDto);
        }

        if (await _userService.ExistsByCedulaAsync(createUserDto.Cedula))
        {
            ModelState.AddModelError("Cedula", "Ya existe un usuario con esta cédula");
            await PopulateRolesDropdown();
            return View(createUserDto);
        }

        if (await _userService.ExistsByUsuarioNombreAsync(createUserDto.Usuario_Nombre))
        {
            ModelState.AddModelError("Usuario_Nombre", "Ya existe un usuario con este nombre de usuario");
            await PopulateRolesDropdown();
            return View(createUserDto);
        }

        await _userService.CreateAsync(createUserDto, "SYSTEM");
        TempData["SuccessMessage"] = "Usuario creado exitosamente";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        var updateUserDto = new UpdateUserDto
        {
            Id = user.Id,
            RoleId = user.RoleId,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Cedula = user.Cedula,
            Usuario_Nombre = user.Usuario_Nombre,
            Fecha_Nacimiento = user.Fecha_Nacimiento
        };

        await PopulateRolesDropdown(user.RoleId);
        return View(updateUserDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateUserDto updateUserDto)
    {
        if (id != updateUserDto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateRolesDropdown(updateUserDto.RoleId);
            return View(updateUserDto);
        }

        if (await _userService.ExistsByCedulaAsync(updateUserDto.Cedula, updateUserDto.Id))
        {
            ModelState.AddModelError("Cedula", "Ya existe un usuario con esta cédula");
            await PopulateRolesDropdown(updateUserDto.RoleId);
            return View(updateUserDto);
        }

        if (await _userService.ExistsByUsuarioNombreAsync(updateUserDto.Usuario_Nombre, updateUserDto.Id))
        {
            ModelState.AddModelError("Usuario_Nombre", "Ya existe un usuario con este nombre de usuario");
            await PopulateRolesDropdown(updateUserDto.RoleId);
            return View(updateUserDto);
        }

        var result = await _userService.UpdateAsync(updateUserDto, "SYSTEM");
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Usuario actualizado exitosamente";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _userService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Usuario eliminado exitosamente";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateRolesDropdown(int? selectedRoleId = null)
    {
        var roles = await _roleService.GetAllAsync();
        ViewBag.Roles = new SelectList(roles, "Id", "Nombre", selectedRoleId);
    }
}
