using AutoMapper;
using ExpressKuryer.Application.DTOs.Subscribe;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribesController : ControllerBase
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        public SubscribesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("BeSub")]
        public async Task<IActionResult> BeSub(SubscribeDto subscribeDto)
        {
            if (RegexManager.CheckMailRegex(subscribeDto.Email) == false)
            {
                return BadRequest("Please fix your email");
            }

            bool check = await _unitOfWork.RepositorySubscribe.IsAny(x=>x.Email.ToLower().Equals(subscribeDto.Email.ToLower()));
            if(check) return BadRequest("This email has been taken");
            var subscribe = _mapper.Map<Subscribe>(subscribeDto);

            await _unitOfWork.RepositorySubscribe.InsertAsync(subscribe);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

    }
}
