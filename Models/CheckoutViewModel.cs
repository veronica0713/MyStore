using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Braintree;
using SmartyStreets.USExtractApi;

namespace MyStore.Models
{
    public class CheckoutViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "State")]
        public string State { get; set; }
        [Required]
        [Display(Name = "Zip")]
        public string Zip { get; set; }
        [Required]
        [Display(Name = "Name on Card")]
        public string NameOnCard { get; set; }
        [Required]
        [MaxLength(16)]
        [Display(Name = "Credit Card Number")]
        public string CCnumber { get; set; }
        [Required]
        [Display(Name = "Expiration month")]
        public string ExpirationMonth { get; set; }
        [Required]
        [Display(Name = "Expiration year")]
        public string ExpirationYear { get; set; }
        [Required]
        [Display(Name = "CVV")]
        public string CVV { get; set; }
        public Cart Cart { get; set; }
        
    }
}
