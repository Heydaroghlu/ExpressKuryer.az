using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.ViewModels
{
    public class DashboardCourierViewModel
    {

        public int CourierId { get; set; }
        public int DeliveryId { get; set; }
        public List<Courier> Couriers { get; set; }
    }
}
