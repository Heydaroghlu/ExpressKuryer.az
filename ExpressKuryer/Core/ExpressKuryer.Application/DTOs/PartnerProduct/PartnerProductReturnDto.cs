using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.PartnerProduct
{
    public class PartnerProductReturnDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool? IsInterestFree { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
