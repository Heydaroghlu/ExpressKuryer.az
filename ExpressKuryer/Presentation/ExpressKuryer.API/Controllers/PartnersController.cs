using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        static string _imagePath = "/uploads/partners/";
        public PartnersController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted, false);

            var returnDto = _mapper.Map<List<PartnerReturnDto>>(entities);

            returnDto.ForEach(x =>
            {
                x.Image = HttpService.StorageUrl(_imagePath, x.Image);
                x.HoverImage = HttpService.StorageUrl(_imagePath, x.HoverImage);
            });

            return Ok(returnDto);
        }

    }
}
