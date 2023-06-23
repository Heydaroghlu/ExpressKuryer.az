using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Dashboard");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            AppUser user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Parol və ya şifrə yanlışdır");
                return View(loginDto);
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Parol və ya şifrə yanlışdır");
                return View(loginDto);
            }
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


        #region Roles
        //[HttpGet]
        //public async Task<IActionResult> Role()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Courier"));
        //    return Ok();
        //}

        //[HttpGet]
        //public async Task<IActionResult> Okay()
        //{
        //    AppUser user = new AppUser
        //    {
        //        UserName = "Admin",
        //        Email = "admin@mail.com",
        //        IsAdmin = true,
        //        EmailConfirmed = true,
        //    };

        //    await _userManager.CreateAsync(user, "Admin123");
        //    await _userManager.AddToRoleAsync(user, "Admin");

        //    return Ok();
        //}
        #endregion

    }
}
