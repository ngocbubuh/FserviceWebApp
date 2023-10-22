﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace NET1705_FService.Repositories.Data
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "Name is required!")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Phone number is required!")]
        [Phone(ErrorMessage = "Must be phone number format!")]
        public required string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required!")]
        public required string Address { get; set; }
        [Required(ErrorMessage = "Date of Birth is required!")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Email is required!"), EmailAddress(ErrorMessage = "Must be email format!")]
        public required string Email { get; set; }
        //[Required, EmailAddress]
        //public string UserName { get; set; } = null!;
        [Required(ErrorMessage = "Password is required!")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required!")]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [PasswordPropertyText]
        public required string ConfirmPassword { get; set; }
    }
}
