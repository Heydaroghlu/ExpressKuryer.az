﻿using ExpressKuryer.Application.DTOs.Delivery;
using ExpressKuryer.Application.UnitOfWorks;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpressKuryer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(DeliveryPost deliveryPost)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new { ModelState,message="Model is not true"});
            }
            Delivery delivery = new Delivery()
            {
                AddressFrom = deliveryPost.From,
                AddressTo = deliveryPost.To,
                Name = deliveryPost.Name,
                Telephone = deliveryPost.Phone,
                Message = deliveryPost.Descripton,
                suprizDelivery = deliveryPost.Supris,
                DeliveryTime = DateTime.UtcNow.AddHours(4),
                DeliveryStatus = Domain.Enums.Delivery.DeliveryStatus.Gözləmədə.ToString(),
                OrderDeliveryStatus = Domain.Enums.Delivery.OrderDeliveryStatus.Anbarda.ToString(),
                TotalAmount = deliveryPost.TotalAmount
            };
            
            if(deliveryPost.AppUserId!=null)
            {
                AppUser user = await _unitOfWork.RepositoryUser.GetAsync(x => x.Id == deliveryPost.AppUserId);
                if(user!=null)
                {
                    delivery.MemberUserId = deliveryPost.AppUserId;
                }
               
            }
            await _unitOfWork.RepositoryDelivery.InsertAsync(delivery);
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                return Ok("Commit Error");
            }
            return StatusCode(201);
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
