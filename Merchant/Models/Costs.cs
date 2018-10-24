using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Merchant.Models
{
    public class Costs
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Category { get; set; }
        [Required]
        [MaxLength(30)]
        public string AmountName { get; set; }
        public decimal Amount { get; set; }

    }
}
