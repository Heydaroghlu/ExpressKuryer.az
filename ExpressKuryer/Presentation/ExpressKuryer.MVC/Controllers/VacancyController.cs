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

        public VacancyController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Vakantlar";

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryVacancy.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<VacancyReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<VacancyReturnDto>.Save(query, page, pageSize);
            List<string> listOfUrls = new List<string>();

            foreach (var item in list)
            {
                var listOfUrl = _storage.GetUrl("uploads/", "vacancies/", item.CV);
                item.CV = listOfUrl;
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var existObject = await _unitOfWork.RepositoryVacancy.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<VacancyReturnDto>(existObject);
            editDto.CV = _storage.GetUrl("uploads/", "sliders/", editDto.CV);
            return View(editDto);
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
    }
}
