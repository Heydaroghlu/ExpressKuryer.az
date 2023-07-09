using ExpressKuryer.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class ProductImages:BaseEntity
    {
        //one to many relations by Isa
        public string? Image { get; set; }
        public bool? IsPoster { get; set; }
        public int? PartnerProductId { get; set; }
        public PartnerProduct? PartnerProduct { get; set; }
    }
}
