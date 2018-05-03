using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MyStore.Models
{
    public class RegisterViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.MinLength(5)]
        [System.ComponentModel.DataAnnotations.MaxLength(10)]
        public string UserName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; }
    }
}
