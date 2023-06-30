using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Slider
{
    public class SliderEditDto
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public IFormFile? FormFile { get; set; }
        public string? Image { get; set; }
    }
}
