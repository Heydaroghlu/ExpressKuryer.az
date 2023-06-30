using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Subscribe
{
    public class SubscribeReturnDto
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
