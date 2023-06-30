using ExpressKuryer.Application.DTOs.JobSeekerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Vacancy
{
    public class VacancyReturnDto
    {

        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<JobSeekerReturnDto>? JobSeekers { get; set; }
        
    }
}
