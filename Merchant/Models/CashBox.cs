using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class CashBox
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public decimal Debit { get; set; }
        public decimal Credit { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
