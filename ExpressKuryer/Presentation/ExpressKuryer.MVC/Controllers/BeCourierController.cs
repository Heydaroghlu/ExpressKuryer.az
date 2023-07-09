using AutoMapper;
using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs.CourierDTOs;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.BeCourierDTOs;

namespace ExpressKuryer.MVC.Controllers
{
    public class BeCourierController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        UserManager<AppUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        IEmailService _emailService;
        IFileService _fileService;
        static string _imagePath = "/uploads/becouriers/";

        public BeCourierController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailService, IFileService fileService)
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
            var entities = await _unitOfWork.RepositoryBeCourier.GetAllAsync(x => true, false);
            int pageSize = 10;


            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryBeCourier.GetAllAsync(x => x.IsDeleted, false);
            else if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryBeCourier.GetAllAsync(x => !x.IsDeleted, false);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.ToLower().Contains(searchWord.ToLower())).ToList();
            }

            var returnDto = _mapper.Map<List<BeCourierReturnDto>>(entities);

            var list = PagenatedList<BeCourierReturnDto>.Save(returnDto.AsQueryable(), page, pageSize);
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Kuryer ol";
            return View(list);
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var beCourier = await _unitOfWork.RepositoryBeCourier.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (beCourier == null)
                return RedirectToAction("NotFound", "Pages");
            var map = _mapper.Map<BeCourierReturnDto>(beCourier);
            TempData["Title"] = "Kuryer ol";
            return View(map);
        }

    }
}
