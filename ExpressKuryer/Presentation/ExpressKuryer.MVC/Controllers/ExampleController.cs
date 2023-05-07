using AutoMapper;
using ExpressKuryer.Application.DTOs.Partner;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.Enums;
using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    public class ExampleController : Controller
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;
        readonly IStorage _storage;

        public ExampleController(IUnitOfWork unitOfWork, IMapper mapper, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storage = storage;
        }
        public async Task<IActionResult> Index(string nameCont, int page = 1, string? searchWord = null, bool? isDeleted = null)
        {

            var entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => true, false);

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => x.Name.Contains(searchWord));
            }

            if (isDeleted == true)
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => x.IsDeleted);
            else
                entities = await _unitOfWork.RepositoryPartner.GetAllAsync(x => !x.IsDeleted);

            int pageSize = 10;

            var returnDto = _mapper.Map<List<PartnerReturnDto>>(entities);

            var query = returnDto.AsQueryable();

            var list = PagenatedList<PartnerReturnDto>.Save(query, page, pageSize);
            ViewBag.PageSize = pageSize;

            return RedirectToAction("Index",nameCont);
        }
    }
}
