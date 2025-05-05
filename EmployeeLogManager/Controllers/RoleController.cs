using EmployeeLogManager.Model.Entities;
using EmployeeLogManager.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("Role")]
public class RoleController : Controller
{
    private readonly IRoleService _service;

    public RoleController(IRoleService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var roles = await _service.GetAllAsync();
        return View(roles);
    }

    [HttpGet("Create")]
    public IActionResult Create() => View(new RoleViewModel());


    [HttpPost("Create")]
    public async Task<IActionResult> Create(RoleViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var existing = await _service.GetAllAsync();
        if (existing.Any(r => r.Name == model.Name))
        {
            ModelState.AddModelError("", "Role name already exists.");
            return View(model);
        }

        var role = new Role
        {
            Name = model.Name
        };

        await _service.CreateAsync(role);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var role = await _service.GetByIdAsync(id);
        if (role == null) return NotFound();

        var viewModel = new RoleViewModel
        {
            Id = role.Id,
            Name = role.Name
        };

        return View(viewModel);
    }

    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit(int id, RoleViewModel viewModel)
    {
        if (!ModelState.IsValid)
            return View(viewModel);

        var role = new Role { Id = id, Name = viewModel.Name };
        await _service.UpdateAsync(role);
        return RedirectToAction(nameof(Index));
    }


    [HttpPost("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
