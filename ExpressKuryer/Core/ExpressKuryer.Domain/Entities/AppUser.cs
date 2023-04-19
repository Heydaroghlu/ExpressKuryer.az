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
        public string Name { get;set; } 
        public string Surname { get; set; } 
        public bool IsAdmin { get; set; }

    }
}
