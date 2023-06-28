using ExpressKuryer.Application.Enums;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.ViewModels
{
    public class DashboardViewModel
    {
        public int MonthPercent { get; set; }
        public int ThisMonth { get; set; }
        public int AllUser { get; set; }
        public List<Delivery> Deliveries { get; set; }
        public int AllDelivery { get; set; }
        public DashboardCourierViewModel DashboardCourierViewModel { get; set; }
        public decimal GetTotalAmount { get; set; }
    }
}
