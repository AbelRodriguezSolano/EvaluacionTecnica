using EvaluacionTecnica.Application.DTOs;
using EvaluacionTecnica.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaluacionTecnica.Web.Controllers;

[Authorize]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var roles = await _roleService.GetAllAsync();
        return View(roles);
    }

    public async Task<IActionResult> Details(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleDto createRoleDto)
    {
        if (!ModelState.IsValid)
        {
            return View(createRoleDto);
        }

        if (await _roleService.ExistsAsync(createRoleDto.Nombre))
        {
            ModelState.AddModelError("Nombre", "Ya existe un rol con este nombre");
            return View(createRoleDto);
        }

        await _roleService.CreateAsync(createRoleDto, "SYSTEM");
        TempData["SuccessMessage"] = "Rol creado exitosamente";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }

        var updateRoleDto = new UpdateRoleDto
        {
            Id = role.Id,
            Nombre = role.Nombre
        };

        return View(updateRoleDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateRoleDto updateRoleDto)
    {
        if (id != updateRoleDto.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(updateRoleDto);
        }

        if (await _roleService.ExistsAsync(updateRoleDto.Nombre, updateRoleDto.Id))
        {
            ModelState.AddModelError("Nombre", "Ya existe un rol con este nombre");
            return View(updateRoleDto);
        }

        var result = await _roleService.UpdateAsync(updateRoleDto, "SYSTEM");
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Rol actualizado exitosamente";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var role = await _roleService.GetByIdAsync(id);
        if (role == null)
        {
            return NotFound();
        }
        return View(role);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _roleService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Rol eliminado exitosamente";
        return RedirectToAction(nameof(Index));
    }
}
