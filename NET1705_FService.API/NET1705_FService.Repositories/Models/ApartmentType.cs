using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class ApartmentType
{
    public int Id { get; set; }

    public int? BuildingId { get; set; }

    public string Type { get; set; }

    public virtual ICollection<Apartment> Apartments { get; } = new List<Apartment>();

    public virtual Building Building { get; set; }

    public virtual ICollection<PackageDetail> PackageDetails { get; } = new List<PackageDetail>();

    public virtual ICollection<Package> Packages { get; } = new List<Package>();
    public virtual ICollection<PackagePrice> PackagePrices { get; } = new List<PackagePrice>();
}
