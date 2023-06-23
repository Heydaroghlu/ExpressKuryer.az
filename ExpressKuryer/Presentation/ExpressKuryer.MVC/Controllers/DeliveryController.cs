using AutoMapper;
using ExpressKuryer.Application.DTOs;
using ExpressKuryer.Application.DTOs.DeliveryDTOs;
using ExpressKuryer.Application.DTOs.Setting;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.Delivery;
using ExpressKuryer.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    public class DeliveryController : Controller
    {
        IUnitOfWork _unitOfWork;
        IEmailService _emailService;
        UserManager<AppUser> _userManager;
        IMapper _mapper;
        public DeliveryController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
            _mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string? searchWord = null, bool? isDeleted = false, string? orderStat = null, string? deliveryStat = null)
        {
            var entities = await _unitOfWork.RepositoryDelivery.GetAllAsync(x => true, false, "Service", "PartnerProduct", "MemberUser", "Courier");
            //todo change to returnDto
            int pageSize = 10;

            orderStat = orderStat ?? DeliveryStatus.Gözləmədə.ToString();
            deliveryStat = deliveryStat ?? OrderDeliveryStatus.Anbarda.ToString();

            if (orderStat.Equals(DeliveryStatus.İmtina)) entities = entities.Where(x => x.DeliveryStatus == DeliveryStatus.İmtina.ToString()).ToList();
            else if (orderStat.Equals(DeliveryStatus.Gözləmədə)) entities = entities.Where(x => x.DeliveryStatus == DeliveryStatus.Gözləmədə.ToString()).ToList();
            else if (orderStat.Equals(DeliveryStatus.Qəbul)) entities = entities.Where(x => x.DeliveryStatus == DeliveryStatus.Qəbul.ToString()).ToList();


            if (deliveryStat.Equals(OrderDeliveryStatus.Anbarda)) entities = entities.Where(x => x.OrderDeliveryStatus == OrderDeliveryStatus.Anbarda.ToString()).ToList();
            else if (deliveryStat.Equals(OrderDeliveryStatus.Kuryerde)) entities = entities.Where(x => x.OrderDeliveryStatus == OrderDeliveryStatus.Kuryerde.ToString()).ToList();
            else if (deliveryStat.Equals(OrderDeliveryStatus.Catdirildi)) entities = entities.Where(x => x.OrderDeliveryStatus == OrderDeliveryStatus.Catdirildi.ToString()).ToList();

            if (isDeleted == true)
                entities = entities.Where(x => x.IsDeleted).ToList();
            if (isDeleted == false)
                entities = entities.Where(x => !x.IsDeleted).ToList();

            if (string.IsNullOrWhiteSpace(searchWord) == false)
            {
                entities = entities.Where(x => x.Name.ToLower().Contains(searchWord.ToLower())).ToList();
            }

            var returnDto = _mapper.Map<List<DeliveryReturnDto>>(entities);
            var list = PagenatedList<DeliveryReturnDto>.Save(returnDto.AsQueryable(), page, pageSize);

            ViewBag.PageSize = pageSize;
            ViewBag.Word = searchWord;
            TempData["Title"] = "Çatdırılmalar";
            TempData["Page"] = page;
            TempData["IsDeleted"] = isDeleted;

            return View(list);
        }


        [HttpGet]
        [Route("Manage/Detail")]
        public async Task<IActionResult> Detail(int id)
        {
            var entity = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == id, false, "Service", "PartnerProduct", "AppUser", "Courier");
            if (entity == null) return NotFound("Order Not Found");

            var returnDto = _mapper.Map<DeliveryReturnDto>(entity);
            return View(returnDto);
        }




        [HttpPost]
        [Route("Manage/ChangeDeliveryStatus")]
        public async Task<IActionResult> ChangeDeliveryStatus(int orderId, string status, string appUserId)
        {
            var entity = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == orderId);
            var appUser = await _userManager.FindByIdAsync(appUserId);

            if (status.Equals(DeliveryStatus.İmtina.ToString())) entity.DeliveryStatus = DeliveryStatus.İmtina.ToString();
            else if (status.Equals(DeliveryStatus.Gözləmədə.ToString())) entity.DeliveryStatus = DeliveryStatus.Gözləmədə.ToString();
            else if (status.Equals(DeliveryStatus.Qəbul.ToString())) entity.DeliveryStatus = DeliveryStatus.Qəbul.ToString();

            await _unitOfWork.CommitAsync();

            _emailService.Send(appUser.Email, $"Sifariş statusu", $"Sizin sifarişiniz statusu {status} olaraq dəyişdirildi");

            object page = TempData["Page"] as int?;
            object word = TempData["Page"] as string;
            object isDeleted = TempData["Page"] as bool?;
            return RedirectToAction("GetAll", new { page = page, searchWord = word, isDeleted = isDeleted });
        }

        [HttpPost]
        [Route("Manage/ChangeOrderDeliveryStatus")]
        public async Task<IActionResult> ChangeOrderDeliveryStatus(int orderId, string status, string appUserId)
        {
            var entity = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == orderId);
            var appUser = await _userManager.FindByIdAsync(appUserId);


            if (status.Equals(OrderDeliveryStatus.Anbarda.ToString())) entity.OrderDeliveryStatus = OrderDeliveryStatus.Anbarda.ToString();
            else if (status.Equals(OrderDeliveryStatus.Kuryerde.ToString())) entity.OrderDeliveryStatus = OrderDeliveryStatus.Kuryerde.ToString();
            else if (status.Equals(OrderDeliveryStatus.Catdirildi.ToString()))
            {
                entity.OrderDeliveryStatus = OrderDeliveryStatus.Catdirildi.ToString();
                var gainPercent = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals("GainPercent"));
                entity.Courier.Gain = (entity.Courier.Gain / 100) * Convert.ToInt32(gainPercent.Value);
            }
            await _unitOfWork.CommitAsync();

            _emailService.Send(appUser.Email, $"Sifariş statusu", $"Sizin sifarişiniz statusu {status} olaraq dəyişdirildi");

            object page = TempData["Page"] as int?;
            object word = TempData["Page"] as string;
            object isDeleted = TempData["Page"] as bool?;
            return RedirectToAction("GetAll", new { page = page, searchWord = word, isDeleted = isDeleted });
        }

        [HttpDelete]
        [Route("Manage/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == id);
            if (entity == null) return NotFound("Order Not Found");

            entity.IsDeleted = true;
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;
            object word = TempData["Page"] as string;
            object isDeleted = TempData["Page"] as bool?;
            return RedirectToAction("GetAll", new { page = page, searchWord = word, isDeleted = isDeleted });
        }

        [HttpPost]
        [Route("Manage/Recover")]
        public async Task<IActionResult> Recover(int id)
        {
            var entity = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == id);
            if (entity == null) return NotFound("Order Not Found");

            entity.IsDeleted = false;
            await _unitOfWork.CommitAsync();

            object page = TempData["Page"] as int?;
            object word = TempData["Page"] as string;
            object isDeleted = TempData["Page"] as bool?;
            return RedirectToAction("GetAll", new { page = page, searchWord = word, isDeleted = isDeleted });
        }

    }
}
