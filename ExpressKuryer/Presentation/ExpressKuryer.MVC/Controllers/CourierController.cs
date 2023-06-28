using AutoMapper;
using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.CourierDTOs;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.Exceptions;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.UserEnums;
using ExpressKuryer.Infrastructure.Services.Email;
using ExpressKuryer.Persistence.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Icao;

namespace ExpressKuryer.MVC.Controllers
{
    public class CourierController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        IEmailService _emailService;
        IFileService _fileService;
        static string _imagePath = "/uploads/users/";

        public CourierController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _fileService = fileService;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryCourier.GetAllAsync(x => true, false, "CourierPerson");
            int pageSize = 10;


            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryCourier.GetAllAsync(x => x.IsDeleted, false, "CourierPerson");
            else if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryCourier.GetAllAsync(x => !x.IsDeleted, false, "CourierPerson");

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.CourierPerson.Name.ToLower().Contains(searchWord.ToLower())).ToList();
            }

            var returnDto = _mapper.Map<List<CourierReturnDto>>(entities);

            var list = PagenatedList<CourierReturnDto>.Save(returnDto.AsQueryable(), page, pageSize);
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Kuryerlər";
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourierCreateDto courierCreateDto)
        {

            if (!ModelState.IsValid) return View(courierCreateDto);

            if (courierCreateDto.FormFile != null)
            {
                try
                {
                    _fileService.CheckFileType(courierCreateDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    return View(courierCreateDto);
                }
            }

            if (!RegexManager.CheckMailRegex(courierCreateDto.CourierPerson.Email))
            {
                ModelState.AddModelError("CourierPerson.Email", "Mail doğru deyil");
                return View(courierCreateDto);
            }

            if (!RegexManager.CheckPhoneRegex(courierCreateDto.CourierPerson.PhoneNumber))
            {
                ModelState.AddModelError("CourierPerson.PhoneNumber", "Telefon doğru deyil");
                return View(courierCreateDto);
            }

            var entity = _mapper.Map<ExpressKuryer.Domain.Entities.Courier>(courierCreateDto);
            var appUser = _mapper.Map<AppUser>(courierCreateDto.CourierPerson);
            appUser.UserName = appUser.Email;

            if (courierCreateDto.FormFile != null)
            {
                var imageInfo = await _storage.UploadAsync(_imagePath, courierCreateDto.FormFile);
                entity.CourierPerson.Image = imageInfo.fileName;
            }

            entity.CourierPerson.UserType = UserRoleEnum.Courier.ToString(); //todo here will change

            var result = await _userManager.CreateAsync(appUser, courierCreateDto.CourierPerson.Password);
            entity.CourierPersonId = appUser.Id;

            await _unitOfWork.RepositoryCourier.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            _emailService.Send(appUser.Email, "Kuryer ol", "Sizin Express Kuryer-də Kuryer olaraq hesabınız yaradıldı");
            //todo emailde error verende xeber cixarsin


            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => x.Id == id, false, "CourierPerson");
            if (courier == null) return RedirectToAction("NotFound", "Pages");

            var returnDto = _mapper.Map<CourierEditDto>(courier);
            TempData["userId"] = courier.CourierPerson.Id;
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);


            return View(returnDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourierEditDto editDto)
        {

            var existObject = await _unitOfWork.RepositoryCourier.GetAsync(x => x.Id == editDto.Id, true, "CourierPerson");

            if (!ModelState.IsValid) return View(editDto);

            if (editDto.FormFile != null)
            {
                try
                {
                    _fileService.CheckFileType(editDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    return View(editDto);
                }
                var check = _storage.HasFile(_imagePath, existObject.CourierPerson.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync(_imagePath, existObject.CourierPerson.Image);
                    var imageInfo = await _storage.UploadAsync(_imagePath, editDto.FormFile);
                    existObject.CourierPerson.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync(_imagePath, editDto.FormFile);
                    existObject.CourierPerson.Image = imageInfo.fileName;
                }
            }

            if (!RegexManager.CheckMailRegex(editDto.CourierPerson.Email))
            {
                ModelState.AddModelError("CourierPerson.Email", "Mail doğru deyil");
                return View(editDto);
            }

            if (!RegexManager.CheckPhoneRegex(editDto.CourierPerson.PhoneNumber))
            {
                ModelState.AddModelError("CourierPerson.PhoneNumber", "Telefon doğru deyil");
                return View(editDto);
            }


            existObject.CourierPerson.Name = editDto.CourierPerson.Name;
            existObject.CourierPerson.Surname = editDto.CourierPerson.Surname;
            existObject.CourierPerson.Address = editDto.CourierPerson.Address;
            existObject.CourierPerson.PhoneNumber = editDto.CourierPerson.PhoneNumber;
            existObject.CourierPerson.Email = editDto.CourierPerson.Email;


            object page = TempData["Page"] as int?;


            return RedirectToAction("Index", new { page = page });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => x.Id == id);
            if (courier == null) return RedirectToAction("NotFound", "Pages");

            courier.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            return View(courier);
        }

        public async Task<IActionResult> Recover(int id)
        {
            var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => x.Id == id);
            if (courier == null) return RedirectToAction("NotFound", "Pages");

            courier.IsDeleted = false;

            await _unitOfWork.CommitAsync();

            return View(courier);
        }

        public async Task<IActionResult> ChangePassword(string userId)
        {
            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null) return RedirectToAction("NotFound", "Pages");

            Random rand = new Random();

            string newPassword = appUser.Name[0].ToString() + appUser.Surname[0].ToString() + rand.Next(0, 9999999).ToString();

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
            var resetResult = await _userManager.ResetPasswordAsync(appUser, resetToken, newPassword);

            if (resetResult.Succeeded)
            {
                TempData["Success"] = "Parol Dəyişildi";
                _emailService.Send(appUser.Email, "Parol Prosesi", $"Sizin Express Kuryer-də hesabızın parolu yeniləndi : {newPassword}");
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("ERROR");
                TempData["Error"] = "Parol Dəyişilmədi!";
            }

            return Ok();
        }

    }
}
