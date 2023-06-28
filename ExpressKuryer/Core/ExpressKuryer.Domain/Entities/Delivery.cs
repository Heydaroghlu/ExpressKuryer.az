using ExpressKuryer.Domain.Entities.Common;
using ExpressKuryer.Domain.Enums.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Delivery:BaseEntity
    {
        public int ServiceId { get; set; }

        public DateTime DeliveryedAt { get; set; }
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public string Name { get; set; }    
        public string Telephone { get; set; }
        public string Message { get;set; }  
        public bool expressDelivery { get; set; }
        public bool suprizDelivery { get; set; }
        public decimal TotalAmount { get; set; } 
        public double DisCount { get; set; }
        public string DeliveryCode { get; set; }
        public string SurName { get; set; } //todo surname 

        public Service Service { get; set; }
        public string DeliveryStatus { get; set; }
        public string OrderDeliveryStatus { get; set; }

        public string MemberUserId { get; set; }
        public AppUser MemberUser { get; set; }

        public int? CourierId { get; set; }
        public Courier?  Courier { get; set; }
    }
}
