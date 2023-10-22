﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Accounts : IdentityUser
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Avatar { get; set; }
    public bool Status { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public virtual ICollection<Apartment> Apartments { get; } = new List<Apartment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
