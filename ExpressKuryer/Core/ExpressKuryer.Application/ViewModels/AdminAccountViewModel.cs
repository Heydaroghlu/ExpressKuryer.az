using ExpressKuryer.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.ViewModels
{
    public class AdminAccountViewModel
    {
        public AppUser? User { get; set; }
        public int? PartnerCount { get; set; }
        public int? UserCount { get; set; }
        public int? CourierCount { get; set; }
        public string? Password { get; set; }
        public IFormFile? FormFile { get; set; }


        public DateTime Now { get; set; } = DateTime.UtcNow.AddHours(4);
        public List<Delivery> MemberDeliveries { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string daterange { get; set; }
        public Courier Courier { get; set; }


        public decimal CourierGain { get; set; }
        public decimal TotalCourierGain { get; set; }

    }
}
