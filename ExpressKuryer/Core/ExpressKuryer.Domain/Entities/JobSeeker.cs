using ExpressKuryer.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class JobSeeker : BaseEntity
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Message { get; set; }
        public string? Cv { get; set; }
        [NotMapped]
        public IFormFile? FormFile { get; set; }

        public int? VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
    }
}
