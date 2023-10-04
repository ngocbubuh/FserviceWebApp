using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public virtual ICollection<Apartment> Apartments { get; } = new List<Apartment>();
}
