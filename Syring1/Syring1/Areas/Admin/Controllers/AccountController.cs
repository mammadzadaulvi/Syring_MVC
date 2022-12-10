using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Syring1.Constants;
using Syring1.Models;
using Syring1.ViewModels.Account;
using System.Data;

namespace Syring1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                return View(model);
            }

            if (!await _userManager.IsInRoleAsync(user, UserRoles.Admin.ToString()))
            {
                ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username or Password is wrong");
                return View(model);
            }
            return RedirectToAction("index", "dashboard");
        }
    }
}
