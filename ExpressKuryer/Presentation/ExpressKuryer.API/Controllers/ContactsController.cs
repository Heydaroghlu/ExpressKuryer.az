using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;


        public ContactsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("sendContact")]
        public async Task<IActionResult> SendContact(ContactDto contactDto)
        {
            if (!ModelState.IsValid) return StatusCode(400, ModelState.ValidationState);

            if (RegexManager.CheckMailRegex(contactDto.Email) == false)
            {
                return StatusCode(400, "Please fix your email");
            }

            if (RegexManager.CheckPhoneRegex(contactDto.Phone) == false)
            {
                return StatusCode(400, "Please fix your phone number");
            }

            var contact = _mapper.Map<Contact>(contactDto);

            await _unitOfWork.RepositoryContact.InsertAsync(contact);
            await _unitOfWork.CommitAsync();

            return Ok(contact);
        }


        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => true, false);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => x.Name.Contains(searchWord) || x.Message.Contains(searchWord));
            }

            if (isDeleted == true)
                entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => x.IsDeleted);
            else
                entities = await _unitOfWork.RepositoryContact.GetAllAsync(x => !x.IsDeleted);


            int pageSize = 10;

            //entities = entities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var returnDto = _mapper.Map<List<ContactReturnDto>>(entities);

            var query = returnDto.AsQueryable();

            var list = PagenatedList<ContactReturnDto>.Save(query, page, pageSize);

            return Ok(entities);
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryContact.GetAsync(x => x.Id == id, false);
            if (existObject == null) return NotFound();
            return Ok(existObject);
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(ContactDto objectDto, int id, int page = 1)
        {
            var existObject = await _unitOfWork.RepositoryContact.GetAsync(x => x.Id == id);

            if (existObject == null) return NotFound();

            if(!ModelState.IsValid) return StatusCode(400, ModelState.ValidationState);

            if (RegexManager.CheckMailRegex(objectDto.Email) == false)
            {
                return StatusCode(400, "Please fix your email");
            }

            if (RegexManager.CheckPhoneRegex(objectDto.Phone) == false)
            {
                return StatusCode(400, "Please fix your phone number");
            }

            existObject.Name = objectDto.Name;
            existObject.Phone = objectDto.Phone;
            existObject.Email = objectDto.Email;
            existObject.Message = objectDto.Message;

            await _unitOfWork.CommitAsync();
            
            return RedirectToAction("GetAll", new { page = page });
        }

    }
}
