using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.Contact;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.HelperManager;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
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

            return Ok(returnDto);
        }


        [HttpGet]
        [Route("Manage/GetAll")]
        //todo change it
        public async Task<IActionResult> GetAll(int page = 1, string? searchWord = null, bool? isDeleted = null)
        {
            var entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => true, false);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => x.Name.Contains(searchWord));
            }

            if (isDeleted == true)
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x=>x.IsDeleted);
            else
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);

            int pageSize = 10;

            var returnDto = _mapper.Map<List<PartnerReturnDto>>(entities);

            var query = returnDto.AsQueryable();

            var list = PagenatedList<PartnerReturnDto>.Save(query, page, pageSize);

            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm]PartnerCreateDto objectDto)
        {

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);

            var partner = _mapper.Map<Partner>(objectDto);
            partner.Image = imageInfo.fileName;

            await _unitOfWork.RepositoryPartner.InsertAsync(partner);
            await _unitOfWork.CommitAsync();

            return Ok(partner);
        }


        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var existObject = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == id, false);
            if (existObject == null) return NotFound();
            return Ok(existObject);
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromForm]PartnerEditDto objectDto, int id, int page = 1)
        {
            var existObject = await _unitOfWork.RepositoryPartner.GetAsync(x => x.Id == id);

            if (existObject == null) return NotFound();

            if (!ModelState.IsValid) return StatusCode(400, ModelState.ValidationState);

            if(objectDto.FormFile != null)
            {
                var check = _storage.HasFile("uploads/partners/", existObject.Image);
                if(check == true)
                {
                    await _storage.DeleteAsync("uploads/partners/", existObject.Image);
                    var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
                else
                {
                    var imageInfo = await _storage.UploadAsync("uploads/partners/", objectDto.FormFile);
                    existObject.Image = imageInfo.fileName;
                }
            }

            existObject.Name = objectDto.Name;
            await _unitOfWork.CommitAsync();

            return Ok(existObject);
        }



    }
}
