using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Contact:BaseEntity 
    {
        public string? Name { get; set; }
        public string? Phone { get; set; } 
        public string? Email { get; set; }
        public string? Message { get;set; }
    }
}
