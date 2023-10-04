using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Floor
{
    public int Id { get; set; }

    public int No { get; set; }

    public int BuildingId { get; set; }

    public virtual ICollection<Apartment> Apartments { get; } = new List<Apartment>();

    public virtual Building Building { get; set; }
}
