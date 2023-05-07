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

namespace ExpressKuryer.MVC.Controllers
{
    public class ServiceController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public ServiceController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryService.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;

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

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartnerCreateDto objectDto)
        {
            if (!ModelState.IsValid) return View(ModelState);

            try
            {
                _storage.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
            }
            catch (Exception)
            {
                ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                return View(objectDto);
            }

            var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);

            var partner = _mapper.Map<Partner>(objectDto);
            partner.Image = imageInfo.fileName;

            await _unitOfWork.RepositoryPartner.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }


    }
}
