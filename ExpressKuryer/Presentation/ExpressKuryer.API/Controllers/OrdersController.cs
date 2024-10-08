﻿using Avon.Infrastructure.Referal;
using ExpressKuryer.Application.DTOs.Delivery;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using ExpressKuryer.Infrastructure.Services.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        IEmailService _emailService;
        public OrdersController(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork=unitOfWork;
            _emailService=emailService;

        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(DeliveryPost deliveryPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { ModelState, message = "Model is not true" });
            }
            // var data = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key == "CompanyGain", false);
            //var data2 = await _unitOfWork.RepositorySetting.GetAsync(x => x.Key == "Depozit", false);
            var data = 1;
            var data2 = 2;
            decimal companyGain = Convert.ToDecimal(data);
            decimal coruierGain = 100 - companyGain;

            Delivery delivery = new Delivery()
            {
                AddressFrom = deliveryPost.From,
                AddressTo = deliveryPost.To,
                Name = deliveryPost.Name,
                Telephone = deliveryPost.Phone,
                Message = deliveryPost.Descripton,
                Type = deliveryPost.Type,
               
                suprizDelivery = deliveryPost.Supris,
                DeliveryTime = DateTime.UtcNow.AddHours(4),
                DeliveryStatus = Domain.Enums.Delivery.DeliveryStatus.Gözləmədə.ToString(),
                OrderDeliveryStatus = Domain.Enums.Delivery.OrderDeliveryStatus.Anbarda.ToString(),
                TotalAmount = deliveryPost.TotalAmount,
                TrackCode = CodeGeneratorManager.Generate(deliveryPost.Name, deliveryPost.Descripton),
                CompanyGain = deliveryPost.TotalAmount - (deliveryPost.TotalAmount * companyGain) / 100,
                CourierGain = deliveryPost.TotalAmount - (deliveryPost.TotalAmount * coruierGain) / 100,

            };
            AppUser user = null;
            if (deliveryPost.AppUserId != null)
            {
                user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == deliveryPost.AppUserId);
                if (user != null)
                {
                    delivery.MemberUserId = deliveryPost.AppUserId;
                    delivery.Email=user.Email;
                }

            }
            if(deliveryPost.Email != null)
            {
                delivery.Email = deliveryPost.Email;
                 _emailService.Send(delivery.Email, "Express Kuryer", delivery.TrackCode);
            }
            await _unitOfWork.RepositoryDelivery.InsertAsync(delivery);
            try
            {
                await _unitOfWork.CommitAsync();
               
            }
            catch (Exception e)
            {
                return BadRequest(e.InnerException.ToString());
            }
            return Ok(new { TrackCode = delivery.TrackCode });
        }
        [HttpGet]
        [Route("OrderGet")]
        public async Task<IActionResult> Get(int id)
        {
 
            var orders = await _unitOfWork.RepositoryDelivery.GetAsync(x => x.Id == id);
            if(orders==null)
            {
                return NotFound();
            }
            return Ok(orders);
        }
        [HttpGet]
        [Route("Track")]
        public async Task<IActionResult> Tracking(string TrackId)
        {
            var order=await _unitOfWork.RepositoryDelivery.GetAsync(x=>x.TrackCode== TrackId);
            if(order==null)
            {
                return NotFound("This order is not exist");
            }
            return Ok(order);
        }
        [HttpGet]
        [Route("Orders")]
        public async Task<IActionResult> GetAll(string AppUserId)
        {
            AppUser user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == AppUserId);
            if(user==null)
            {
                return NotFound("This user is not exist");
            }
            var orders=await _unitOfWork.RepositoryDelivery.GetAllAsync(x=>x.MemberUserId== user.Id);
            return Ok(orders);  
        }
    }
}
