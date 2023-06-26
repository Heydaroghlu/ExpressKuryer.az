using AutoMapper;
using ExpressKuryer.Application.Abstractions.File;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;

namespace ExpressKuryer.MVC.Controllers
{
    public class SettingController : Controller
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        IFileService _fileService;
        public SettingController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage, IFileService fileService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
            _fileService = fileService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, string? isDeleted = "false")
        {
            var entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => true, false);
            int pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            ViewBag.IsDeleted = isDeleted;

            if (isDeleted == "true")
                entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => x.IsDeleted);
            if (isDeleted == "false")
                entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => !x.IsDeleted);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Value.Contains(searchWord)).ToList();
            }
            
            var returnDto = _mapper.Map<List<SettingReturnDto>>(entities);
            var query = returnDto.AsQueryable();

            var list = PagenatedList<SettingReturnDto>.Save(query, page, pageSize);

            list.ForEach(x =>
            {
                if (x.Key.Contains("image"))
                {
                    var path = _storage.GetUrl("/uploads/settings/", x.Value);
                    x.Value = path;
                };
            });

            ViewBag.PageSize = pageSize;

            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {   
            var setting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Id == id, false);
            if (setting.Key.Contains("image")) setting.Value = _storage.GetUrl("/uploads/settings/", setting.Value);
            return View(setting);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SettingDto settingDto, int id, int page = 1)
        {

            var existSetting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key == settingDto.Key);

            if (existSetting == null) return NotFound();

            if (existSetting.Key.Contains("image") == true)
            {

                if (settingDto.FormFile != null)
                {
                    bool check = _storage.HasFile("/uploads/settings/", existSetting.Value);

                    try
                    {
                        _fileService.CheckFileType(settingDto.FormFile, ContentTypeManager.ImageContentTypes);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("FormFile", ContentTypeManager.ImageContentMessage());
                        return View(settingDto);
                    }

                    if (check == true)
                    {
                        await _storage.DeleteAsync("/uploads/settings/", existSetting.Value);
                        var item = await _storage.UploadAsync("/uploads/settings/", settingDto.FormFile);
                        existSetting.Value = item.fileName;
                    }
                    else
                    {
                        var item = await _storage.UploadAsync("/uploads/settings/", settingDto.FormFile);
                        existSetting.Value = item.fileName;
                    }
                }
            }
            else
                existSetting.Value = settingDto.Value;

            await _unitOfWork.CommitAsync();

            return RedirectToAction("Index", new { page = page });
        }
    }
}
