using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.Subscribe;
using ExpressKuryer.Application.HelperManager;

namespace ExpressKuryer.MVC.Controllers
{
    public class SubscribeController : Controller
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public SubscribeController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositorySubscribe.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositorySubscribe.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositorySubscribe.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Email.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<SubscribeReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<SubscribeReturnDto>.Save(query, page, pageSize);

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var exist = await _unitOfWork.RepositorySubscribe.GetAsync(x => x.Id == id);
            if (exist == null) return RedirectToAction("NotFound", "Pages");
            return View(exist);
        }

        [HttpPost]  
        public async Task<IActionResult> Edit(SubscribeEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositorySubscribe.GetAsync(x => x.Id == id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");

            if (!ModelState.IsValid) return View(objectDto);

            if (RegexManager.CheckMailRegex(objectDto.Email.ToLower()) == false)
            {
                ModelState.AddModelError("Email", "please fix email");
                return View(objectDto);
            }

            existObject.Email = objectDto.Email;
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<IActionResult> Delete(int id)
        {

            var entity = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

    }
}
