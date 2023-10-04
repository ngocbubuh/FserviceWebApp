using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? ApartmentPackageId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string PaymentMethod { get; set; }

    public string CustomerName { get; set; }

    public string Phone { get; set; }

    public string Note { get; set; }

    public DateTime? PaymentDate { get; set; }

    public double? TotalPrice { get; set; }

    public int? PackageId { get; set; }

    public DateTime? StartDate { get; set; }

    public virtual ApartmentPackage ApartmentPackage { get; set; }

    public virtual ICollection<ApartmentPackage> ApartmentPackages { get; } = new List<ApartmentPackage>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
