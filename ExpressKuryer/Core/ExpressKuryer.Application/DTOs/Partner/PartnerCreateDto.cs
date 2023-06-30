using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Partner
{
    public class PartnerCreateDto
    {
        public string? Name { get; set; }
        public IFormFile? FormFile { get; set; }
    }
}
