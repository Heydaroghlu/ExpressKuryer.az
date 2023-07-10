using ExpressKuryer.Application.Abstractions.Token;
using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Application.DTOs.Delivery;
using ExpressKuryer.Application.DTOs.Token;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.UserEnums;
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
        IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(UserManager<AppUser> userManager,IEmailService emailService,IUnitOfWork unitOfWork,ITokenHandler tokenHandler, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _unitOfWork= unitOfWork;
        }
        [HttpGet("createrole")]
        public async Task<IActionResult> CreateRoles()
        {
            var role1 = new IdentityRole("Member");
            var role2 = new IdentityRole("Admin");

            await _roleManager.CreateAsync(role1);
            await _roleManager.CreateAsync(role2);
            return Ok();
        }
        [HttpPost("daxilol")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            AppUser user=await _userManager.FindByNameAsync(login.UserName);
            if(user==null)
            {
                return StatusCode(401, "Ad və ya şifrə yanlışdır !");
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, false, false);
            if (!result.Succeeded)
            {
                return StatusCode(401, "Ad və ya şifrə yanlışdır !");
            }
            TokenDTO token = _tokenHandler.CreateAccessToken(user,60);
            return Ok(token);

        }
        [HttpGet]
        [Route("Users")]
        public async Task<IActionResult> Users()
        {
            var users = await _unitOfWork.RepositoryUser.GetAllAsync(x => x.IsAdmin != true);
            return Ok(users);
        }
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return StatusCode(201, "Ugurla hesabdan cixildi");
        }
        [HttpPost("hesab/qeydiyyat")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            AppUser exist = await _userManager.FindByNameAsync(register.Email);
            if (exist != null)
            {
                return BadRequest("This User is exist");
            }
            if (register.Password != register.RepeatPassword)
            {
                return BadRequest("Password is invalid");
            }
            if (!RegexManager.CheckMailRegex(register.Email))
            {
                return BadRequest("Email is invalid");
            }
            if (!RegexManager.CheckPhoneRegex(register.Phone))
            {
                return BadRequest("Phone is invalid");
            }
            AppUser user = new()
            {
                Name = register.Name,
                Surname = register.Surname,
                Email = register.Email,
                PhoneNumber = register.Phone,
                Address = register.Address,
                UserName = register.Email,
                UserType = UserRoleEnum.Member.ToString()
            };
            var result = await _userManager.CreateAsync(user, register.Password);
            await _userManager.AddToRoleAsync(user, "Member");
            return Ok(result);
        }
        [HttpGet("myaccount")]
        public async Task<IActionResult> MyAccount(string id)
        {
            AppUser user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == id, false, "Deliveries");
            var data = await _unitOfWork.RepositoryDelivery.GetAllAsync(x => x.MemberUserId == id);
            if (user == null)
            {
                return NotFound();
            }
            AccountDTO accountDTO = new AccountDTO()
            {
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                Email = user.Email,
                Deliveries = data.ToList(),
                Phone = user.PhoneNumber
            };
            return Ok(accountDTO);
        }
        [HttpPost("updateAccount")]
        public async Task<IActionResult> MyAccount(string id,RegisterDTO register)
        {
            AppUser user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == id,true);
            if (user == null)
            {
                return BadRequest();
            }
            user.Surname = register.Surname;
            user.Address = register.Address;
            user.Name=register.Name;
            user.PhoneNumber = register.Phone;
            AccountDTO accountDTO = new AccountDTO()
            {
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                Email = user.Email,
                Deliveries = user.Deliveries,
                Phone = user.PhoneNumber
            };
            if (register.Password != null && register.Password==register.RepeatPassword)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, register.Password);
                if (!resetResult.Succeeded)
                {
                    return BadRequest("Password error!");
                }
                _emailService.Send(user.Email, $"Hörmətli müştəri {user.Name} ", $"Sizin şifrəniz {DateTime.UtcNow.AddHours(4)} tarixində dəyişdirildi.Yeni şifrəniz: {register.Password} .Təşəkkürlər");
            }
            await _unitOfWork.CommitAsync();
            return Ok(accountDTO);
        }
     /*   [HttpGet("forgot")]
        public async Task<IActionResult> Forgot(string Email)
        {
            AppUser user = await _userManager.FindByNameAsync(Email);
            if (user == null)
            {
                return Unauthorized();
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passurl = Url.Action(action: "ChangePassword", controller: "Account", new { id = user.Id, token }, Request.Scheme);
            _emailService.Send(user.Email, "Yeni Şifrə", passurl);
            return Ok();
        }*/
      /*  [HttpPut]
        public async Task<IActionResult> ChangePassword(string id,string token)
        {
            
        }*/
      
    }
}
