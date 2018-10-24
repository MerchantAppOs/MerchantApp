using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
     public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Surname { get; set; }
        [Required]
        [MaxLength(30)]
        public string FatherName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        public DateTime StartDay { get; set; }
        [Required]
        public decimal Salary { get; set; }
    }
}
