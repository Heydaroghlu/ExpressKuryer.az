using AutoMapper;
using ExpressKuryer.Application.DTOs.Vacancy;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.JobSeekerDTOs;

namespace ExpressKuryer.MVC.Controllers
{
    public class JobSeekerController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        static string _imagePath = "/uploads/jobSeekers/";
        public JobSeekerController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryJobSeeker.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Vakantlar";

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryJobSeeker.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryJobSeeker.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<JobSeekerReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<JobSeekerReturnDto>.Save(query, page, pageSize);
            List<string> listOfUrls = new List<string>();

            foreach (var item in list)
            {
                var listOfUrl = _storage.GetUrl(_imagePath, item.CV);
                item.CV = listOfUrl;
            }

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var existObject = await _unitOfWork.RepositoryJobSeeker.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<JobSeekerReturnDto>(existObject);
            editDto.CV = _storage.GetUrl(_imagePath, editDto.CV);
            return View(editDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryJobSeeker.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }
    }
}
