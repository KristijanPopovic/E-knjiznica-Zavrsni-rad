﻿using System.ComponentModel.DataAnnotations;

namespace E_knjiznica.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }  // ✅ Dodano ovo svojstvo
    }
}
