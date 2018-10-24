using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class Products
    {
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductCode { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductName { get; set; }
        [MaxLength(30)]
        public string Mark { get; set; }
        [Required]
        public string Unit { get; set; }
        public string Barcode { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public decimal SellingPrice { get; set; }
        public int LimitedAmount { get; set; }
    }
}
