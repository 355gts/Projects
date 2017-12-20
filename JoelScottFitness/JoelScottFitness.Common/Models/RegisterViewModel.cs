﻿using System.ComponentModel.DataAnnotations;

namespace JoelScottFitness.Common.Models
{
    public class RegisterViewModel : CreateCustomerViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The passwords entered do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
