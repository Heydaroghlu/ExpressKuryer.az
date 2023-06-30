using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Service:BaseEntity
    {
        public string? Name { get; set; }
        public double? Depozit { get; set; } 
        public string? Icon { get;set; }
        public double? OwnAvragePercent { get; set; }
    }
}
