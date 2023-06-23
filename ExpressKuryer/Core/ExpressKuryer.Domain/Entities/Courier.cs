using ExpressKuryer.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Domain.Entities
{
    public class Courier : BaseEntity
    {
        public decimal Gain { get; set; }

        public string CourierPersonId { get; set; }
        public AppUser CourierPerson { get; set; }

        [NotMapped]
        List<Delivery> Deliveries { get; set; }
    }
}
