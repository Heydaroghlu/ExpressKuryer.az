using AutoMapper;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {

        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        static string _imagePath = "/uploads/sliders/";
        public SlidersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var sliders = await _unitOfWork.RepositorySlider.GetAllAsync(x => !x.IsDeleted, false);
            
            var returnDto = _mapper.Map<SliderReturnDto>(sliders);
            returnDto.Image = HttpService.StorageUrl(_imagePath, returnDto.Image);
            return Ok(returnDto);
        }

    }
}
