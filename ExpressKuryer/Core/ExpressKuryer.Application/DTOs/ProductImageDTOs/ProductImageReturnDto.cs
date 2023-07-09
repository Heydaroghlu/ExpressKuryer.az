using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.ProductImageDTOs
{
    public class ProductImageReturnDto
    {
        public string? Image { get; set; }
        public bool? IsPoster { get; set; }
        public int? PartnerProductId { get; set; }
    }
}
