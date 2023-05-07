using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.Slider;

namespace ExpressKuryer.MVC.Controllers
{
    public class SliderController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public SliderController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => x.IsDeleted);
            if(isDeleted == "false")
                entities = await _unitOfWork.RepositorySlider.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Title.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<SliderReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<SliderReturnDto>.Save(query, page, pageSize);
            List<string> listOfUrls = new List<string>();

            foreach (var item in list)
            {
                var listOfUrl = _storage.GetUrl("uploads/", "sliders/", item.Image);
                item.Image = listOfUrl;
            }

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
                _storage.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
            }
            catch (Exception)
            {
                ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                return View(objectDto);
            }

            var imageInfo = await _storage.UploadAsync("uploads/sliders/", objectDto.FormFile);

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
            editDto.Image = _storage.GetUrl("uploads/", "sliders/", editDto.Image);
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
                    _storage.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    objectDto.Image =  _storage.GetUrl("uploads/", "sliders/", existObject.Image);
                    return View(objectDto);
                }
                var check = _storage.HasFile("uploads/sliders/", existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync("uploads/sliders/", existObject.Image);
                    var imageInfo = await _storage.UploadAsync("uploads/sliders/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync("uploads/sliders/", objectDto.FormFile);
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
