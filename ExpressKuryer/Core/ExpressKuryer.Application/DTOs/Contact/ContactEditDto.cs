﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Contact
{
    public class ContactEditDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
