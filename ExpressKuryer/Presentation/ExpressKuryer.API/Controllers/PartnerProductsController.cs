using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerProductsController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;
        static string _imagePath = "/uploads/partnerProducts/";
        public PartnerProductsController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _unitOfWork.RepositoryPartnerProduct.GetAsync(x => x.Id == id && !x.IsDeleted, false,"ProductImages","Partner");

            var returnDto = _mapper.Map<PartnerProductReturnDto>(entity);

            returnDto.Image = HttpService.StorageUrl(_imagePath, returnDto.Image);
            returnDto.ProductImages.ForEach(x =>
            {
                x.Image = HttpService.StorageUrl(_imagePath, x.Image);
            });
            return Ok(returnDto);
        }

    }
}
