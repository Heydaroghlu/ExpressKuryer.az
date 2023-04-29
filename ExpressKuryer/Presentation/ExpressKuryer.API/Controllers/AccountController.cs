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
        public AccountController(UserManager<AppUser> userManager, EmailService emailService, ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
        [HttpPost("hesab/qeydiyyat")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            AppUser exist = await _userManager.FindByNameAsync(register.Email);
            if (exist != null)
            {
                return BadRequest(register);
            }
            if (register.Password != register.RepeatPassword)
            {
                return BadRequest(register);
            }
            if (!RegexManager.CheckMailRegex(register.Email))
            {
                return BadRequest(register);
            }
            if (!RegexManager.CheckPhoneRegex(register.Phone))
            {
                return BadRequest(register);
            }
            AppUser user = new()
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                PhoneNumber = register.Phone,
                Address = register.Address,
                UserName = register.Email
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            await _userManager.AddToRoleAsync(user, "Member");
            return Ok(result);
        }
        [HttpGet("myaccount")]
        public async Task<IActionResult> MyAccount(string id)
        {
            AppUser user = await _userManager.Users.Include(x => x.Deliveries).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest();
            }
            AccountDTO accountDTO = new AccountDTO()
            {
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                Email = user.Email,
                Deliveries = user.Deliveries,
                Phone = user.PhoneNumber
            };
            return Ok(accountDTO);
        }
        [HttpGet("forgot")]
        public async Task<IActionResult> Forgot(string Email)
        {
            AppUser user = await _userManager.FindByNameAsync(Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passurl = Url.Action(action: "MyAccount", controller: "Account", new { email = user.Email, token }, Request.Scheme);
            _emailService.Send(user.Email, "Yeni Şifrə", passurl);
            return Ok();
        }
        public async Task<IActionResult> ResetPassword(ResetPassDTO resetPassDTO)
        {
            if (resetPassDTO.Email == null)
            {
                return BadRequest();
            }
            AppUser user = await _userManager.FindByNameAsync(resetPassDTO.Email);
            if (user == null || !(await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPassDTO.Token)))
            {
                return BadRequest();
            }
        }
    }
}
