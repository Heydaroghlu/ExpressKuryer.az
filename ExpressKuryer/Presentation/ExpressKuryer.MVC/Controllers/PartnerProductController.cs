using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Domain.Entities;

namespace ExpressKuryer.MVC.Controllers
{
    public class PartnerProductController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public PartnerProductController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int partnerId, int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryPartnerProduct.GetAllAsync(x => x.PartnerId == partnerId, false);
            var partner = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == partnerId);
            int pageSize = 10;

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            ViewBag.PartnerName = partner.Name;
            TempData["PartnerId"] = partnerId;

            if (isDeleted == "true")
                entities = entities.Where(x => x.IsDeleted).ToList();
            if (isDeleted == "false")
                entities = entities.Where(x => !x.IsDeleted).ToList();

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.Contains(searchWord)).ToList();
            }

            var returnDto = _mapper.Map<List<PartnerProductReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<PartnerProductReturnDto>.Save(query, page, pageSize);
            //todo remove this list
            List<string> listOfUrls = new List<string>();

            foreach (var item in list)
            {
                var listOfUrl = _storage.GetUrl("uploads/", "partnerProducts/", item.Image);
                item.Image = listOfUrl;
            }

            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartnerProductCreateDto objectDto)
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

            var imageInfo = await _storage.UploadAsync("uploads/partnerProducts/", objectDto.FormFile);

            var partner = _mapper.Map<PartnerProduct>(objectDto);
            partner.Image = imageInfo.fileName;

            await _unitOfWork.RepositoryPartnerProduct.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;
            partner.PartnerId = (int)TempData["PartnerId"];

            return RedirectToAction("Index", new { partnerId = partner.PartnerId, page = page });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id, false);
            if (existObject == null) return RedirectToAction("NotFound", "Page");

            var editDto = _mapper.Map<PartnerProductEditDto>(existObject);
            editDto.Image = _storage.GetUrl("uploads/", "partnerProducts/", editDto.Image);
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PartnerProductEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");


            //todo change to rule to all conrtroller Image url can be return
            objectDto.Image = _storage.GetUrl("uploads/", "partnerProducts/", existObject.Image);

            if (!ModelState.IsValid) return Ok(ModelState);

            if (objectDto.FormFile != null)
            {
                try
                {
                    _storage.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    objectDto.Image = _storage.GetUrl("uploads/", "partnerProducts/", existObject.Image);
                    return View(objectDto);
                }

                var check = _storage.HasFile("uploads/partnerProducts/", existObject.Image);
                if (check == true)
                {
                    await _storage.DeleteAsync("uploads/partnerProducts/", existObject.Image);
                    var imageInfo = await _storage.UploadAsync("uploads/partnerProducts/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync("uploads/partnerProducts/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
            }

            existObject.Name = objectDto.Name;
            existObject.CostPrice = objectDto.CostPrice;
            existObject.SalePrice = objectDto.SalePrice;
            existObject.Description = objectDto.Description;
            existObject.DiscountPrice = objectDto.DiscountPrice;
            existObject.IsInterestFree = objectDto.IsInterestFree;
            
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;
            int partnerId = (int)TempData["PartnerId"];

            return RedirectToAction("Index", new { partnerId = partnerId, page = page });
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
