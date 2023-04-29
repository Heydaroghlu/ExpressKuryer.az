﻿using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    public class SettingController : Controller
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public SettingController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => true, false);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => x.Value.Contains(searchWord));
            }

            if (isDeleted == true)
                entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => x.IsDeleted);
            else
                entities = await _unitOfWork.RepositorySetting.GetAllAsync(x => !x.IsDeleted);

            int pageSize = 10;

            var returnDto = _mapper.Map<List<SettingReturnDto>>(entities);

            var query = returnDto.AsQueryable();

            var list = PagenatedList<SettingReturnDto>.Save(query, page, pageSize);
            ViewBag.PageSize = pageSize;

            return View(list);
        }




        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {   
            var setting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Id == id, false);
            return View(setting);
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(SettingDto settingDto, int id, int page = 1)
        {

            var existSetting = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key == settingDto.Key);

            if (existSetting == null) return NotFound();

            if (existSetting.Key.Contains("Image") == true)
            {

                if (settingDto.FormFile != null)
                {
                    bool check = _storage.HasFile("uploads/settings/", existSetting.Value);

                    if (check == true)
                    {
                        await _storage.DeleteAsync("uploads/settings/", existSetting.Value);
                        var item = await _storage.UploadAsync("uploads/settings/", settingDto.FormFile);
                        existSetting.Value = item.fileName;
                    }
                    else
                    {
                        var item = await _storage.UploadAsync("uploads/settings/", settingDto.FormFile);
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