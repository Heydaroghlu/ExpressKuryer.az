using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.Abstractions.File;
using CloudinaryDotNet.Actions;

namespace ExpressKuryer.MVC.Controllers
{
    public class SliderController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        IFileService _fileService;
        static string _imagePath = "/uploads/sliders/";
        public SliderController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
        }
        //todo image temasini deyis!
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => true, false);
            int pageSize = 10;
          

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => x.IsDeleted);
            if(isDeleted == "false")
                entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Title.Contains(searchWord)).ToList();
            }

            var list = PagenatedList<Slider>.Save(entities.AsQueryable(), page, pageSize);

            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["Title"] = "Sliderlər";

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateDto objectDto)
        {
            if (!ModelState.IsValid) return View(objectDto);

            try
            {
                _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
            }
            catch (Exception)
            {
                ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                return View(objectDto);
            }

            var imageInfo = await _storage.UploadAsync(_imagePath, objectDto.FormFile);

            var entity = _mapper.Map<Slider>(objectDto);
            entity.Image = imageInfo.fileName;

            await _unitOfWork.RepositorySlider.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<SliderEditDto>(existObject);
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);

            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SliderEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");

            if (!ModelState.IsValid) return View(objectDto);

            if (objectDto.FormFile != null)
            {
                try
                {
                    _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    objectDto.Image =  _storage.GetUrl(_imagePath, existObject.Image);
                    return View(objectDto);
                }
                var check = _storage.HasFile(_imagePath, existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync(_imagePath, existObject.Image);
                    var imageInfo = await _storage.UploadAsync(_imagePath, objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync(_imagePath, objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
            }

            existObject.Title = objectDto.Title;
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositorySlider.GetAsync(x => x.Id == id);

            if (entity == null) return RedirectToAction("NotFound", "Page");

            entity.IsDeleted = true;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }
    }
}
