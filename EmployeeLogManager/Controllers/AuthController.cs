using EmployeeLogManager.Model.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EmployeeLogManager.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLogManager.Controllers
{
    public class AuthController : Controller
    {

        private readonly EmployeeLogManagerDbcontext _context;

        public AuthController(EmployeeLogManagerDbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .Include(x => x.Role) // 👈 FIXED: this loads the role object
                    .FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);

                if (user != null)
                {
                    var claims = new List<Claim>
            {
                         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name , user.Email),
                new Claim(ClaimTypes.Role , user.Role.Name)
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    if (user.Role.Name == "Admin")
                        return RedirectToAction("Index", "Admin");
                    else
                        return RedirectToAction("Index", "Dailylog");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is not valid.");
                }
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                ViewBag.Message = "Email not found.";
                return View();
            }

            // You can implement sending email logic here if needed
            ViewBag.Message = "If this email exists, a reset link has been sent.";
            return View();
        }

    }
}


/*Note: 
1. Claims are temporary user info(key-value pair) used during the session.
2. They’re stored in an encrypted authentication cookie in the user's browser so we only include(email and role) and don't include the password in claims for security reasons.*/
/*ClaimsIdentity encapsulates information about the user,*/