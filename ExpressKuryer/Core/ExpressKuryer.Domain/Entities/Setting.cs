using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Setting
    {
        public string Key { get; set; } 
        public string Value { get; set; }
        [NotMapped]
        public IFormFile FormFile { get; set; } 
    }
}
