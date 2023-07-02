using AutoMapper;
using ExpressKuryer.Application.DTOs.Vacancy;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacanciesController : ControllerBase
    {

        readonly IStorage _storage;
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public VacanciesController(IStorage storage, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _storage = storage;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /*[HttpPost]
        [Route("SendVacancy")]
        public async Task<IActionResult> SendVacancy([FromForm]VacancyDto vacancyDto)
        {

            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (vacancyDto.FormFile == null) return BadRequest("File must be fill");

            var contentInfo = await _storage.UploadAsync("uploads/vacancies/", vacancyDto.FormFile, "application/pdf");

            var entity = _mapper.Map<Vacancy>(vacancyDto);
            entity.Cv = contentInfo.fileName;

            await _unitOfWork.RepositoryVacancy.InsertAsync(entity);
            await _unitOfWork.CommitAsync();


            return Ok(entity);
        }*/

    }
}
