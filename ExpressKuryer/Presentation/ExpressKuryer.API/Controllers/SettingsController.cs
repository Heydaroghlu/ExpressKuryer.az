﻿using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        readonly IStorage _storage;
        static string _imagePath = "/uploads/settings/";
        public SettingsController(IMapper mapper, IUnitOfWork unitOfWork, IStorage storage)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _storage = storage;
        }


        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(string key)
        {
            var setting =  await _unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals(key),false);

            if (setting == null) return NotFound();
            
            var returnDto = _mapper.Map<SettingReturnDto>(setting);

            if (key.Contains("image"))
            {
                returnDto.Value = HttpService.StorageUrl(_imagePath, returnDto.Value);
            }

            return Ok(returnDto);
        }

        

    }
}
