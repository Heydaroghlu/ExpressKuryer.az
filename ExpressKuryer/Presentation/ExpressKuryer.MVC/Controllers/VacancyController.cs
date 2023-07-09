using AutoMapper;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.Vacancy;

namespace ExpressKuryer.MVC.Controllers
{
    public class VacancyController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        static string _imagePath = "/uploads/vacancies";
        readonly IWebHostEnvironment _env;
        readonly IConfiguration _configuration;
        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IWebHostEnvironment env, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _env = env;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => true, false);
            int pageSize = 10;


            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Title.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<VacancyReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<VacancyReturnDto>.Save(query, page, pageSize);
            
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Vakansiyalar";

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VacancyCreateDto objectDto)
        {
            if (!ModelState.IsValid) return View(objectDto);

            var entity = _mapper.Map<Vacancy>(objectDto);

            await _unitOfWork.RepositoryVacancy.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryVacancy.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<VacancyEditDto>(existObject);
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(VacancyEditDto objectDto)
        {
            var existObject = await _unitOfWork.RepositoryVacancy.GetAsync(x => x.Id == objectDto.Id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");

            if (!ModelState.IsValid) return View(objectDto);

            existObject.Title = objectDto.Title;
            existObject.Description = objectDto.Description;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }



        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryVacancy.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }



        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> imageFiles)
        {
            var file = Request.Form.Files.First();

            var fileName = await _storage.UploadAsync(_imagePath, file);

            var filePath = "";

            if (_env.IsDevelopment())
            {
                filePath = "https://localhost:7283/" + _imagePath  + fileName;
                filePath = _storage.GetUrl(_imagePath, fileName.fileName);
            }
            else
            {
                filePath = "http://aliyusifov.com/" + _imagePath + fileName;
                filePath = _storage.GetUrl(_configuration.GetSection("WebSiteURL").Value, fileName.fileName);
            }

            return Json(new { url = filePath });
        }

    }
}
