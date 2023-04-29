using AutoMapper;
using ExpressKuryer.Application.DTOs.Slider;
using ExpressKuryer.Application.UnitOfWorks;
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

            return Ok(returnDto);
        }

    }
}
