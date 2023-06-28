using ExpressKuryer.Application.Storages;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Application.ViewModels;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Domain.Enums.UserEnums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpressKuryer.MVC.Controllers
{
    public class AnalyticsController : Controller
    {

        UserManager<AppUser> _userManager;
        IUnitOfWork _unitOfWork;
        static string _imagePath = "/uploads/analiytics/";
        IStorage _storage;
        public AnalyticsController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IStorage storage)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _storage = storage;
        }

        public async Task<IActionResult> Index()
        {
            var top5UserWhoDeliveryed = _userManager.Users.Where(x => x.UserType == UserRoleEnum.Member.ToString()).OrderByDescending(x=>x.Deliveries.Count()).Take(5).Include("Deliveries").ToList();

            //var a = _userManager.Users.OrderByDescending(u => u.Deliveries.Count).ToList();

            AnalyticsViewModel viewModel = new AnalyticsViewModel()
            {
                Top5UserWhoDeliveryed = top5UserWhoDeliveryed
            };

            TempData["Title"] = "Analitika";
            TempData["ImagePath"] = _storage.GetUrl(_imagePath, null);
            return View(viewModel);
        }
    }
}
