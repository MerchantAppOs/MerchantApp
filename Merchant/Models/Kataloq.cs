using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class Kataloq
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string CatalogName { get; set; }
        [Required]
        [MaxLength(30)]
        public string Code { get; set; }
    }
}
