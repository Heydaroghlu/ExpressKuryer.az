using ExpressKuryer.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class PartnerProduct:BaseEntity 
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile PosterImage { get; set;}
        [NotMapped]
        public List<ProductImages> ProductImages { get; set; }
        [NotMapped]
        public List<int> ProductImageIds { get; set; } = new List<int>();
        public decimal? SalePrice { get; set; }
        public decimal? CostPrice { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool? IsInterestFree { get; set; }

        public Partner? Partner { get; set; }
        public int? PartnerId { get; set; }
    }
}
