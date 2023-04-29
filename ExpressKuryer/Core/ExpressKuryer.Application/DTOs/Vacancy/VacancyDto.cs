using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Vacancy
{
    public class VacancyDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Message { get; set; }

        public IFormFile FormFile { get; set; }

    }
}
