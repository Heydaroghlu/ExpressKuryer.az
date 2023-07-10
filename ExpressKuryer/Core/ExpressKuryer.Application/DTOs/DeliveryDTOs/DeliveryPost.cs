using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Delivery
{
    public class DeliveryPost
    {
        public string From { get; set; }
        public string To { get; set; }
        public bool Supris { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public string Descripton { get; set; }
        public decimal TotalAmount { get; set; }
        public string? AppUserId { get; set; }

    }
}
