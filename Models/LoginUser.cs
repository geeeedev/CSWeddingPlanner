using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSWeddingPlanner.Models
{
    [NotMapped]
    public class LoginUser
    {
        [Display(Name="Email Address")]
        [Required(ErrorMessage="Missing!"), EmailAddress(ErrorMessage=" is not in correct format.")]
        public string LoginEmail { get; set; }   

        [Display(Name="Password")]
        [Required(ErrorMessage="Missing!"),]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}

