using EmployeeLogManager.BLL.Services;
using EmployeeLogManager.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
[Route("user")]
public class UserController : Controller
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var users = await _service.GetActiveUsersAsync();
        return View(users);
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.userRoles = await _service.GetRolesAsync();
        return View(new UserViewModel());
    }

    //The GET method only runs once — when the user first visits the form.
    //while post must manually re-prepare all data required by the view(like dropdown lists).

    [HttpPost("create")]
    public async Task<IActionResult> Create(UserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.userRoles = await _service.GetRolesAsync();
            return View(model);
        }

        var (success, message) = await _service.CreateUserAsync(model);
        if (!success)
        {
            ViewBag.ErrorMsg = message;
            ModelState.AddModelError("", message);
            ViewBag.userRoles = await _service.GetRolesAsync();
            return View(model);
        }

        ViewBag.SuccessMsg = message;
        ViewBag.userRoles = await _service.GetRolesAsync(); 
        return View("Index");
    }

    [HttpGet("edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _service.GetUserByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> Edit(UserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var (success, message) = await _service.UpdateUserAsync(model);
        if (!success)
        {
            ModelState.AddModelError("", message);
            return View(model);
        }

        TempData["SuccessMsg"] = message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, message) = await _service.SoftDeleteUserAsync(id);
        TempData["SuccessMsg"] = message;
        return RedirectToAction(nameof(Index));
    }
}


/* <---------------NOTE------------->
Find: Only work with the primary key; only hit the DB if it has not already been loaded into memory.
FirstOrDefault: query from any column or combination(not by PK), hits the database even if the entity is already tracked in memory.
TempData: TempData serve redirectional but viewBag/ViewData do not survive redirects (they only work during the same request).*/ 