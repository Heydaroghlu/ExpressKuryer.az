﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.AppUserDTOs
{
    public class RegisterDTO
    {
        public string? Name { get; set;}
        public string? Surname { get; set; }
        public string? Address { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public string? RepeatPassword { get; set; }



    }
}
