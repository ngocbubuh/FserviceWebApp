using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Building
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<ApartmentType> ApartmentTypes { get; } = new List<ApartmentType>();

    public virtual ICollection<Floor> Floors { get; } = new List<Floor>();
}
