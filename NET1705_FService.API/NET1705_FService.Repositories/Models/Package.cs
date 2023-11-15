using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Package
{
    public int Id { get; set; }

    public string UnsignName { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double? Price { get; set; }

    public int? Duration { get; set; }

    public string Image { get; set; }

    //public int? TypeId { get; set; }

    public int? DisplayIndex { get; set; }

    public bool Status { get; set; }

    public virtual ICollection<ApartmentPackage> ApartmentPackages { get; } = new List<ApartmentPackage>();

    public virtual ICollection<PackageDetail> PackageDetails { get; set; } = new List<PackageDetail>();

    public virtual ICollection<PackagePrice> PackagePrices { get; set; } = new List<PackagePrice>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    //public virtual ApartmentType? Type { get; set; }
}
