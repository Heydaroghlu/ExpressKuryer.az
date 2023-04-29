using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Contact
{
    public class ContactReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public DateTime UpadtedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
