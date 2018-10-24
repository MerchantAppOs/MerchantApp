using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Code { get; set; }
        [Required]
        public decimal DebtFromUs { get; set; }
        [Required]
        public decimal DebtToUs { get; set; }

        public bool JuridicalPerson { get; set; }

        public bool IndividualPerson { get; set; }

        [Required]
        public int PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public decimal LoanLimited { get; set; }

        public decimal Discount { get; set; }

        public bool Sale { get; set; }

        public bool Buy { get; set; }
        [MaxLength(250)]
        public string Note { get; set; }
        [Required]
        public DateTime RegistrationTime { get; set; }
        public bool Activity { get; set; }
    }
}
