using AutoMapper;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagesController : ControllerBase
    {

        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;


        public PagesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("sendContact")]
        public async Task<IActionResult> Contact(ContactDto contactDto)
        {
            if (!ModelState.IsValid) return StatusCode(400,ModelState.ValidationState);

            if (RegexManager.CheckMailRegex(contactDto.Email) == false)
            {
                return StatusCode(404, "Please fix your email");
            }

            if (RegexManager.CheckPhoneRegex(contactDto.Phone))
            {
                return StatusCode(404, "Please fix your phone number");
            }

            var contact = _mapper.Map<Contact>(contactDto);
            
            await _unitOfWork.RepositoryContact.InsertAsync(contact);
            await _unitOfWork.CommitAsync();

            return Ok(contact);
        }

    }
}
