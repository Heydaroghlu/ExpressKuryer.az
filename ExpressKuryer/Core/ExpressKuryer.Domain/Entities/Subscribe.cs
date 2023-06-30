using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Subscribe:BaseEntity
    {
        public string? Email { get; set; } 
    }
}
