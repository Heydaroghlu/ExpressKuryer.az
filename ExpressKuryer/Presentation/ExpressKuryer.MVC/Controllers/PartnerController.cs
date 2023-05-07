using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExpressKuryer.MVC.Controllers
{
    public class PartnerController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public PartnerController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index( int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);


            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }


            var returnDto = _mapper.Map<List<PartnerReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<PartnerReturnDto>.Save(query, page, pageSize);
            List<string> listOfUrls = new List<string>();

            foreach(var item in list)
            {
                var listOfUrl = _storage.GetUrl("uploads/","partners/",item.Image);
                item.Image = listOfUrl;
            }

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

            return RedirectToAction("Index",new { page = page });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound","Page");

            var editDto = _mapper.Map<PartnerEditDto>(existObject);
            editDto.Image = _storage.GetUrl("uploads/", "partners/", editDto.Image);
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PartnerEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == id);

            if (existObject == null) return RedirectToAction("NotFound","Page");

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
                    objectDto.Image = _storage.GetUrl("uploads/", "partners/", existObject.Image);
                    return View(objectDto);
                }

                var check = _storage.HasFile("uploads/partners/", existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync("uploads/partners/", existObject.Image);
                    var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
            }

            existObject.Name = objectDto.Name;
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index",new {page = page});
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
