using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using E_knjiznica.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("AdminDashboard", "Home");

                    return RedirectToAction("UserDashboard", "Home");
                }
            }
        }
        ModelState.AddModelError("", "Neispravni podaci.");
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    [Authorize(Roles = "Admin")]
    public IActionResult AdminDashboard() => View();

    [Authorize(Roles = "User,Admin")]
    public IActionResult UserDashboard() => View();
}
