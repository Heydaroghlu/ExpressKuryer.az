using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressKuryer.Application.DTOs.Setting
{
    public class SettingReturnDto
    {
        public int? Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? WhoIsModified { get; set; }
    }
}
