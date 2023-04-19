using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Paket : BaseEntity
    {
        public string Type { get; set; }    
        public double Discount { get; set; }    
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
