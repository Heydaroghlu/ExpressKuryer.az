using ExpressKuryer.Application.DTOs.AppUserDTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.CourierDTOs
{
    public class CourierEditDto
    {
        public int Id { get; set; }
        public IFormFile? FormFile { get; set; }
        public string? OldPassword { get; set; }
        public AppUserReturnDto CourierPerson { get; set; }
    }
}
