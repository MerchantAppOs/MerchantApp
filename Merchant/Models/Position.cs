using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class Position
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string PositionName { get; set; }
        [Required]
        [DefaultValue(0)]
        public int CountEmployees { get; set; }
    }
}
