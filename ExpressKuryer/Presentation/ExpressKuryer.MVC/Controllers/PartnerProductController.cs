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
using CloudinaryDotNet.Actions;

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
            List<PartnerProduct> entities = await _unitOfWork.RepositoryPartnerProduct.GetAllAsync(x => !x.IsDeleted, false, "Partner");

            if (partnerId != 0)
            {
                entities = await _unitOfWork.RepositoryPartnerProduct.GetAllAsync(x => !x.IsDeleted && x.PartnerId == partnerId, false, "Partner");
            }
            int pageSize = 10;

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
            ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);
            TempData["Title"] = "Məhsul Partnuyorları";

            if (partnerId != 0)
            {
                ViewBag.Partner = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == partnerId);
                ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted && x.Id != partnerId);
            }

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

            ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);

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

            if (objectDto.ImageFiles != null)
            {
                foreach (var item in objectDto.ImageFiles)
                {
                    try
                    {
                        _fileService.CheckFileType(item, ContentTypeManager.ImageContentTypes);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("ImageFiles", ContentTypeManager.ImageContentMessage());
                        return View(objectDto);
                    }
                }

            }

            var imageInfo = await _storage.UploadAsync(_imagePath, objectDto.FormFile);

            var partner = _mapper.Map<PartnerProduct>(objectDto);
            partner.Image = imageInfo.fileName;

            await _unitOfWork.RepositoryPartnerProduct.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            if (objectDto.ImageFiles != null)
            {
                foreach (var item in objectDto.ImageFiles)
                {
                    imageInfo = await _storage.UploadAsync(_imagePath, item);
                    ProductImages productImages = new()
                    {
                        Image = imageInfo.fileName,
                        IsPoster = false,
                        PartnerProductId = partner.Id,
                    };
                    await _unitOfWork.RepositoryProductImages.InsertAsync(productImages);
                }
            }

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;

            return RedirectToAction("Index", new { page = page });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id, false, "ProductImages");
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
            ViewBag.Partners = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);
            var existObject = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id);

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
                    return View(objectDto);
                }
            }

            if (objectDto.ImageFiles != null)
            {

                foreach (var item in objectDto.ImageFiles)
                {
                    try
                    {
                        _fileService.CheckFileType(item, ContentTypeManager.ImageContentTypes);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("ImageFiles", ContentTypeManager.ImageContentMessage());
                        return View(objectDto);
                    }
                }
            }

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

            if (objectDto.ImageFiles != null)
            {
                foreach (var item in objectDto.ImageFiles)
                {
                    var imageInfo = await _storage.UploadAsync(_imagePath, item);
                    ProductImages productImages = new()
                    {
                        Image = imageInfo.fileName,
                        IsPoster = false,
                        PartnerProductId = existObject.Id,
                    };
                    await _unitOfWork.RepositoryProductImages.InsertAsync(productImages);
                }
            }

            existObject.Name = objectDto.Name;
            existObject.CostPrice = objectDto.CostPrice;
            existObject.SalePrice = objectDto.SalePrice;
            existObject.Description = objectDto.Description;
            existObject.DiscountPrice = objectDto.DiscountPrice;

            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;
            TempData["Success"] = "Dəyişikliklər qeydə alındı";
            return RedirectToAction("edit",existObject.Id);
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
                filePath = _storage.GetUrl(_configuration.GetSection("WebSiteURL").Value, fileName.fileName);
            }

            return Json(new { url = filePath });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            try
            {
                var fileName = _unitOfWork.RepositoryProductImages.GetAsync(x => x.Id == id).Result.Image;
                await _unitOfWork.RepositoryProductImages.Remove(x => x.Id == id);
                await _unitOfWork.CommitAsync();


                await _storage.DeleteAsync(_imagePath, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Json("OK");
        }
    }
}