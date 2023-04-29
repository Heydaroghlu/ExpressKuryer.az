using ExpressKuryer.Application.Abstractions.Token;
using ExpressKuryer.Application.DTOs.AppUser;
using ExpressKuryer.Application.DTOs.Token;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Infrastructure.Services.Email;
using ExpressKuryer.Infrastructure.Services.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<AppUser> userManager,EmailService emailService,ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }
        [HttpPost("daxilol")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            AppUser user=await _userManager.FindByNameAsync(login.UserName);
            if(user==null)
            {
                return Unauthorized();
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized();
            }
            TokenDTO token = _tokenHandler.CreateAccessToken(user,60);
            return Ok(token);

        }
    }
}
