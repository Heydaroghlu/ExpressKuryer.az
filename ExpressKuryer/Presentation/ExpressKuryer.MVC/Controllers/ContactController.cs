using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ExpressKuryer.MVC.Controllers
{
    public class ContactController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public ContactController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
           
            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Email.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<ContactReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<ContactReturnDto>.Save(query, page, pageSize);


            TempData["searchWord"] = searchWord;
            TempData["isDeleted"] = isDeleted;
            TempData["Title"] = "Əlaqələr";
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var existObject = await _unitOfWork.RepositoryContact.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<ContactEditDto>(existObject);
			return View(editDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryContact.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }
    }
}
