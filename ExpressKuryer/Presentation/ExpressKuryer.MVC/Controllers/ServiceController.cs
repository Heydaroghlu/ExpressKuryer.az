using AutoMapper;
using ExpressKuryer.Application.DTOs.Subscribe;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs.Slider;

namespace ExpressKuryer.MVC.Controllers
{
    public class ServiceController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        IFileService _fileService;
        public ServiceController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryService.GetAllAsync(x => true, false);
            int pageSize = 10;
          

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryService.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryService.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<ServiceReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<ServiceReturnDto>.Save(query, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Servis";

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceCreateDto objectDto)
        {
            if (!ModelState.IsValid) return View(ModelState);

            var partner = _mapper.Map<Service>(objectDto);

            await _unitOfWork.RepositoryService.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryService.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<ServiceEditDto>(existObject);
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceEditDto objectDto)
        {
            var existObject = await _unitOfWork.RepositoryService.GetAsync(x => x.Id == objectDto.Id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");

            if (!ModelState.IsValid) return View(objectDto);

            existObject.Name = objectDto.Name;
            existObject.OwnAvragePercent = objectDto.OwnAvragePercent;
            existObject.Depozit = objectDto.Depozit;
            existObject.Icon = objectDto.Icon;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

    }
}
