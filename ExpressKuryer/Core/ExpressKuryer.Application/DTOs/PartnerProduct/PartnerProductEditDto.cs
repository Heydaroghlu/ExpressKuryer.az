using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.PartnerProduct
{
    public class PartnerProductEditDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public IFormFile? FormFile { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPrice { get; set; }
        public bool IsInterestFree { get; set; }

        public int PartnerId { get; set; }
        public ExpressKuryer.Domain.Entities.Partner Partner { get; set; }
    }
}
