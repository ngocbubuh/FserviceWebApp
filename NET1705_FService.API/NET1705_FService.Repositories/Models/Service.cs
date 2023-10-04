using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Image { get; set; }

    public virtual ICollection<ApartmentPackageService> ApartmentPackageServices { get; } = new List<ApartmentPackageService>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<PackageDetail> PackageDetails { get; } = new List<PackageDetail>();

    //public virtual ICollection<ServiceJoinStaff> ServiceJoinStaffs { get; } = new List<ServiceJoinStaff>();
}
