using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.CourierDTOs
{
    public class CourierCreateDto
    {
        public IFormFile? FormFile { get; set; }
        public AppUserCreateDto? CourierPerson { get; set; }
    }
}
