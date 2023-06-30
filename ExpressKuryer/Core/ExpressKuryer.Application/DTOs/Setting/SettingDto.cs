using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Setting
{
    public class SettingDto
    {
        public string? Key { get; set; }
        public string? Value { get; set; }

        public IFormFile? FormFile { get; set; }
    }
}
