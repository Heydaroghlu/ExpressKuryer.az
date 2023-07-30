using AutoMapper;
using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.ServiceTwoDTOs;

namespace ExpressKuryer.MVC.Controllers
{
    public class ServiceTwoController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        IFileService _fileService;
        public ServiceTwoController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryServiceTwo.GetAllAsync(x => true, false);
            int pageSize = 10;


            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryServiceTwo.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryServiceTwo.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<ServiceTwoReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<ServiceTwoReturnDto>.Save(query, page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Xidmətlər";

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceTwoCreateDto objectDto)
        {
            if (!ModelState.IsValid) return View(objectDto);

            var partner = _mapper.Map<ServiceTwo>(objectDto);

            await _unitOfWork.RepositoryServiceTwo.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryServiceTwo.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<ServiceTwoEditDto>(existObject);
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ServiceTwoEditDto objectDto)
        {
            var existObject = await _unitOfWork.RepositoryServiceTwo.GetAsync(x => x.Id == objectDto.Id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");

            if (!ModelState.IsValid) return View(objectDto);

            existObject.Name = objectDto.Name;
            existObject.Icon = objectDto.Icon;
            existObject.Description = objectDto.Description;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryServiceTwo.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

    }
}
