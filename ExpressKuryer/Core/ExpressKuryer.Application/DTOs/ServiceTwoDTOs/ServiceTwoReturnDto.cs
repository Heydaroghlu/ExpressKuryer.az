﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.ServiceTwoDTOs
{
    public class ServiceTwoReturnDto
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
    }
}