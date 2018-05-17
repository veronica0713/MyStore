using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        public string Country { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

        public string State { get; set; }
        public int Zip { get; set; }
        [Required]
        public string NameOnCard { get; set; }
        [Required]
        public int CCnumber { get; set; }
        [Required]
        public string Expiration { get; set; }
        [Required]
        public int CVV { get; set; }


    }
}
