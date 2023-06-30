using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.JobSeekerDTOs
{
    public class JobSeekerReturnDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Message { get; set; }
        public string? CV { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
