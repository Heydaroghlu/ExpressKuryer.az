using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        //AppUser
        public string Name { get;set; } 
        public string Surname { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
        public List<Delivery> Deliveries { get; set; }

    }
}
