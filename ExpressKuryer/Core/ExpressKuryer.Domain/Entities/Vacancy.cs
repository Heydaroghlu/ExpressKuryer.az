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
    public class Vacancy:BaseEntity
    {

        public string Title { get; set; }
        public string Description { get; set; }
        

        public List<JobSeeker> JobSeekers { get; set; }
    }
}
