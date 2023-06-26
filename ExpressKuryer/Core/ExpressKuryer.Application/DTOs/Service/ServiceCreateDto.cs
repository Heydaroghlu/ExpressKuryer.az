using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Service
{
    public class ServiceCreateDto
    {
        public string Name { get; set; }
        public double Depozit { get; set; }
        public string Icon { get; set; }
        public double OwnAvragePercent { get; set; }
    }
}
