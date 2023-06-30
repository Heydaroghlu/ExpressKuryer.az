using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.AppUserDTOs
{
    public class ResetPassDTO
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
    }
}
