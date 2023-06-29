using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Application.Abstractions.File;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ExpressKuryer.MVC.Controllers
{
    public class PartnerProductController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        IFileService _fileService;
        static string _imagePath = "/uploads/partnerProducts/";
        IWebHostEnvironment _env;
        IConfiguration _configuration;
        public PartnerProductController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService, IWebHostEnvironment env, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
            _env = env;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int partnerId, int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositoryPartnerProduct.GetAllAsync(x => x.PartnerId == partnerId, false);
            var partner = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == partnerId);
            int pageSize = 10;

            //ViewBag.PartnerName = partner.Name;
            
            
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

            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);


            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;
            TempData["PartnerId"] = partnerId;
            TempData["Title"] = "Məhsul Partnuyorları";

            if (!partnerId.Equals(0))
                TempData["Title"] = $"{partner.Name} Məhsulları";

            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PartnerProductCreateDto objectDto)
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
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);
            ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);
            //todo partner product u check et
            return View(editDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PartnerProductEditDto objectDto, int id)
        {
            var existObject = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id);

            if (existObject == null) return RedirectToAction("NotFound", "Page");


            //todo change to rule to all conrtroller Image url can be return
            objectDto.Image = _storage.GetUrl(_imagePath, existObject.Image);

            if (!ModelState.IsValid) return Ok(ModelState);

            if (objectDto.FormFile != null)
            {
                try
                {
                    _fileService.CheckFileType(objectDto.FormFile, ContentTypeManager.ImageContentTypes);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                    objectDto.Image = _storage.GetUrl(_imagePath, existObject.Image);
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


        [HttpPost]
        public async Task<IActionResult> UploadImage(List<IFormFile> imageFiles)
        {
            var file = Request.Form.Files.First();

            var fileName = await _storage.UploadAsync(_imagePath, file);

            var filePath = "";

            if (_env.IsDevelopment())
            {
                filePath = "https://localhost:7105/" + "/uploads/blogs/" + fileName;
                filePath = _storage.GetUrl(_imagePath, fileName.fileName);
            }
            else
            {
                filePath = "http://aliyusifov.com/" + "/uploads/blogs/" + fileName;
                filePath = _storage.GetUrl(_configuration.GetSection("WebSiteURL").Value,fileName.fileName);
            }

            return Json(new { url = filePath });
        }


    }
}
