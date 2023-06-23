using ExpressKuryer.Application.DTOs.AppUserDTOs;
using ExpressKuryer.Application.DTOs.DeliveryDTOs;
using ExpressKuryer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.CourierDTOs
{
    public class CourierReturnDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Gain { get; set; }

        public string CourierPersonId { get; set; }
        public AppUserReturnDto CourierPerson { get; set; }

        [NotMapped]
        List<DeliveryReturnDto> Deliveries { get; set; }
    }
}
