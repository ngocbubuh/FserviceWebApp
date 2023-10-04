using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class ApartmentPackage
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ApartmentId { get; set; }

    public int? PackageId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; }

    public virtual Apartment Apartment { get; set; }

    public virtual ICollection<ApartmentPackageService> ApartmentPackageServices { get; } = new List<ApartmentPackageService>();

    public virtual Order Order { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Package Package { get; set; }
}
