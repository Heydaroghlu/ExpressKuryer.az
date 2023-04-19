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
        public string AddressFrom { get; set; }
        public string AddressTo { get; set; }
        public string Name { get; set; }    
        public string Telephone { get; set; }
        public string Message { get;set; }  
        public bool expressDelivery { get; set; }
        public bool suprizDelivery { get; set; }
        public double TotalAmount { get; set; } 
        public double DisCount { get; set; }
        public Service Service { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public OrderDeliveryStatus OrderDeliveryStatus { get; set; }
        public int PartnerProductId { get; set; }
        public PartnerProduct PartnerProduct { get; set; }  
    }
}
