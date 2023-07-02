﻿using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Application.ViewModels;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.UserEnums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpressKuryer.MVC.Controllers
{
    public class AccountController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        IUnitOfWork _unitOfWork;
        IFileService _fileService;
        IStorage _storage;
        static string _imagePath = "/uploads/users/";
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IUnitOfWork unitOfWork, IFileService fileService, IStorage storage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _storage = storage;
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

        [HttpGet]
        public async Task<IActionResult> Profile(string? id = null)
        {
            AppUser memberUser = null;
            AppUser user = null;
            AdminAccountViewModel viewModel = new AdminAccountViewModel()
            {

            };

            if (id != null)
            {
                memberUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (memberUser == null) return RedirectToAction("Page", "NotFound");

                var deliveries = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.MemberUserId == memberUser.Id).Result.ToList();
                viewModel.MemberDeliveries = deliveries;

                var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => !x.IsDeleted && x.CourierPersonId == memberUser.Id, false, "CourierPerson");
                viewModel.Courier = courier;

                viewModel.User = memberUser;
                viewModel.MemberDeliveries = await _unitOfWork.RepositoryDelivery.GetAllAsync(x => x.MemberUserId == memberUser.Id);


                var gainPercent = Convert.ToDecimal(_unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals("GainPercent")).Result.Value);

                viewModel.Courier.Gain = 0;

                foreach (var item in deliveries)
                {
                    var result = ((item.TotalAmount * gainPercent) / 100);
                    viewModel.Courier.Gain = viewModel.Courier.Gain + result;
                }


            }
            else
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Page", "NotFound");
                
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                viewModel.User = user;
                viewModel.PartnerCount = _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted).Result.Count();
                viewModel.CourierCount = _unitOfWork.RepositoryCourier.GetAllAsync(x => !x.IsDeleted).Result.Count();
                viewModel.UserCount = _userManager.Users.Where(x => !x.IsAdmin && x.UserType.Equals(UserRoleEnum.Member)).Count();
            }

            TempData["ImagePath"] = _storage.GetUrl(_imagePath,null);

            return View(viewModel);
        }

		[HttpPost]
        public async Task<IActionResult> Profile(AdminAccountViewModel viewModel)
        {

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == viewModel.User.Email.ToLower());
            if (user == null) return NotFound();

            if (user.IsAdmin)
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Page", "NotFound");

                string fileName = null;
                if (viewModel.FormFile != null)
                {
                    try
                    {
                        _fileService.CheckFileType(viewModel.FormFile, ContentTypeManager.ImageContentTypes);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                        return View(viewModel);
                    }
                    var imageInfo = await _storage.UploadAsync(_imagePath, viewModel.FormFile);
                    fileName = imageInfo.fileName;
                }

                if (fileName != null)
                {
                    user.Image = fileName;
                }

                user.Email = viewModel.User.Email;
                user.PhoneNumber = viewModel.User.PhoneNumber;

                if (viewModel.Password != null)
                {

                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await _userManager.ResetPasswordAsync(user, resetToken, viewModel.Password);
                    if (!resetResult.Succeeded)
                    {
                        TempData["Error"] = "Parol Dəyişilmədi!";
                        return View(viewModel);
                    }
                }

                await _unitOfWork.CommitAsync();
                //todo imagePathlere bax

                viewModel = new()
                {
                    CourierCount = _unitOfWork.RepositoryCourier.GetAllAsync(x => !x.IsDeleted).Result.Count(),
                    PartnerCount = _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted).Result.Count(),
                    UserCount = _userManager.Users.Where(x => !x.IsAdmin && x.UserType.Equals(UserRoleEnum.Member)).Count(),
                    User = user
                };

                TempData["Success"] = "Dəyişikliklər qeydə alındı";

            }
            else
            {
                var dateTimes = viewModel.daterange.Split(" - ");

                DateTime startTime = DateTime.ParseExact(dateTimes[0], "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime endTime = DateTime.ParseExact(dateTimes[1], "MM/dd/yyyy", CultureInfo.InvariantCulture);

                var deliveries = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.MemberUserId == user.Id && x.CreatedAt > startTime && x.CreatedAt < endTime).Result.ToList();
                viewModel.MemberDeliveries = deliveries;

                var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => !x.IsDeleted && x.CourierPersonId == user.Id, false, "CourierPerson");
                viewModel.Courier = courier;

                var gainPercent = Convert.ToDecimal(_unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals("GainPercent")).Result.Value);

                viewModel.Courier.Gain = 0;

                foreach (var item in deliveries)
                {
                    var result = ((item.TotalAmount * gainPercent) / 100);
                    viewModel.Courier.Gain = viewModel.Courier.Gain + result;
                }

                TempData["daterange"] = viewModel.daterange;

            }


            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);
            
            return View(viewModel);
        }


       
        //[HttpGet]
        //public async Task<IActionResult> Role()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Courier"));
        //    return Ok();
        //}

        [HttpGet]
        public async Task<IActionResult> Okay()
        {
            AppUser user = new AppUser
            {
                UserName = "AliBagishli",
                Email = "expresskuryer.az@mail.com",
                IsAdmin = true,
                EmailConfirmed = true,
            };

            await _userManager.CreateAsync(user, "Admin0910");
            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok();
        }
     

    }
}
