﻿using ExpressKuryer.Application.DTOs.PartnerProduct;
using ExpressKuryer.Application.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.DeliveryDTOs
{
    public class DeliveryReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ServiceId { get; set; }
        public string AppUserId { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public string Message { get; set; }
        public bool expressDelivery { get; set; }
        public bool suprizDelivery { get; set; }
        public double TotalAmount { get; set; }
        public double DisCount { get; set; }
        public ServiceReturnDto Service { get; set; }
        public string DeliveryStatus { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public int PartnerProductId { get; set; }
        public PartnerProductReturnDto PartnerProduct { get; set; }
        public ExpressKuryer.Domain.Entities.AppUser AppUser { get; set; }

    }
}