using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class ServiceTwo:BaseEntity
    {
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
    }
}
