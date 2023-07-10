using ExpressKuryer.Application.DTOs.CourierDTOs;
using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.DTOs.Service;
using ExpressKuryer.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.DeliveryDTOs
{
    public class DeliveryReturnDto
    {

        //todo delivery code

        public string? DeliveryCode { get; set; }
        public int? Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeliveryedAt { get; set; }
        public int? ServiceId { get; set; }
        public string? AppUserId { get; set; }
        public string? AddressFrom { get; set; }
        public string? AddressTo { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; } 
        public string? Telephone { get; set; }
        public string? Message { get; set; }
        public bool expressDelivery { get; set; }
        public bool suprizDelivery { get; set; }
        public double? TotalAmount { get; set; }
        public double? DisCount { get; set; }
        public ServiceReturnDto? Service { get; set; }
        public string? DeliveryStatus { get; set; }
        public string? OrderDeliveryStatus { get; set; }

        public string Type { get; set; }
        public CourierReturnDto? Courier { get; set; }
        public ExpressKuryer.Domain.Entities.AppUser? MemberUser { get; set; }

        public DashboardCourierViewModel? DashboardCourierViewModel { get; set; }

    }
}
