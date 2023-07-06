using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Application.ViewModels;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.Delivery;
using ExpressKuryer.Domain.Enums.UserEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.MVC.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        IUnitOfWork _unitOfWork;
        UserManager<AppUser> _userManager;
        public DashboardController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var monthly = await GetMonthlyDeliveries();
            var allDelivery = await GetAllDeliveries();
            var allUser = GetAllUsers();
            var deliveries = await GetTodayDeliveries();
            var couriers = await _unitOfWork.RepositoryCourier.GetAllAsync(x => !x.IsDeleted, false, "CourierPerson");
            var totalAmount = await GetTotalAmount();
            Console.WriteLine(totalAmount);
            ViewBag.Couriers = couriers;

            DashboardViewModel viewModel = new()
            {
                AllUser = allUser,
                Deliveries = deliveries,
                MonthPercent = monthly.percent,
                ThisMonth = monthly.thisMonth,
                AllDelivery = allDelivery,
                GetTotalAmount = totalAmount,
                DashboardCourierViewModel = new DashboardCourierViewModel
                {
                    Couriers = couriers,
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(int courierId, int deliveryId)
        {
            var monthly = await GetMonthlyDeliveries();
            var allDelivery = await GetAllDeliveries();
            var allUser = GetAllUsers();
            var deliveries = await GetTodayDeliveries();
            var couriers = await _unitOfWork.RepositoryCourier.GetAllAsync(x => !x.IsDeleted, false, "CourierPerson");
            ViewBag.Couriers = couriers;

            DashboardViewModel viewModel = new()
            {
                AllUser = allUser,
                Deliveries = deliveries,
                MonthPercent = monthly.percent,
                ThisMonth = monthly.thisMonth,
                AllDelivery = allDelivery,
                DashboardCourierViewModel = new DashboardCourierViewModel
                {
                    Couriers = couriers,
                }
            };

            var delivery = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == deliveryId && !x.IsDeleted);
            var courier = await _unitOfWork.RepositoryCourier.GetAsync(x => x.Id == courierId && !x.IsDeleted);
            if (courier != null)
            {
                delivery.CourierId = courierId;
            }

            await _unitOfWork.CommitAsync();

            TempData["Success"] = "Çatdırılmaya Kuryer əlavə olundu";


            return View(viewModel);
        }

        private async Task<(int thisMonth, int percent)> GetMonthlyDeliveries()
        {
            var thisMonth = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.CreatedAt.Month == DateTime.Now.Month && x.CreatedAt.Year == DateTime.Now.Year && x.OrderDeliveryStatus.Equals(OrderDeliveryStatus.Catdirildi.ToString())).Result.Count();
            int preMonth = 0;
            int percent = 0;
            if (DateTime.Now.Month == 1)
                preMonth = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.CreatedAt.Month == 12 && x.CreatedAt.Year == (DateTime.Now.Year - 1) && x.OrderDeliveryStatus.Equals(OrderDeliveryStatus.Catdirildi.ToString())).Result.Count();
            else
                preMonth = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.CreatedAt.Month == 12 && x.CreatedAt.Year == (DateTime.Now.Year - 1) && x.OrderDeliveryStatus.Equals(OrderDeliveryStatus.Catdirildi.ToString())).Result.Count();
            if (preMonth == 0)
                percent = 0;
            else
                percent = (preMonth * 100 / preMonth) - 100;

            //this 50 - x
            //pre 40 - 100

            // 5*100/4 = 125 +25%

            //this 40 - x
            //pre 50 - 100

            // 40*100/50 = 80 -20%  

            return (thisMonth, percent);
        }

        private async Task<int> GetAllDeliveries()
        {
            return _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted).Result.Count();
        }

        private int GetAllUsers()
        {
            return _userManager.Users.Where(x => x.UserType == UserRoleEnum.Member.ToString()).Count();
        }

        private async Task<List<Delivery>> GetTodayDeliveries()
        {
            var date = DateTime.Now.Date;
            var ass = await _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.CreatedAt.Date == DateTime.Now.Date , true, "MemberUser");
            return ass.ToList();
        }

        private async Task<decimal> GetTotalAmount()
        {
            var totalAmount = _unitOfWork.RepositoryDelivery.GetAllAsync(x => !x.IsDeleted && x.OrderDeliveryStatus == OrderDeliveryStatus.Catdirildi.ToString()).Result.Select(x => x.TotalAmount).Sum();
          
            decimal percent = Convert.ToDecimal(_unitOfWork.RepositorySetting.GetAsync(x => x.Key.Equals("GainPercent")).Result.Value);

            // 10* 100 = 80x

            //total amount - 13,000 --- 100
            //               x       -  10


            //todo gain temasini hesabla 
            //todo email gondermek
            return totalAmount - (totalAmount * percent / 100);
        }



    }
}
