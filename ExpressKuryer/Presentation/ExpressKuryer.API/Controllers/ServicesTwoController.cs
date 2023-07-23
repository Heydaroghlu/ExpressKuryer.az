using AutoMapper;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesTwoController : ControllerBase
    {
        readonly IMapper _mapper;
        readonly IUnitOfWork _unitOfWork;
        public ServicesTwoController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _unitOfWork.RepositoryServiceTwo.GetAllAsync(x => x.IsDeleted == false);
            List<ServiceTwoReturnDTO> dto = _mapper.Map<List<ServiceTwoReturnDTO>>(data);
            return Ok(dto);
        }
    }
}
