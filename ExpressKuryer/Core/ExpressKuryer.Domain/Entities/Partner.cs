﻿using ExpressKuryer.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Partner:BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public IFormFile FormFile { get; set; } 
    }
}