using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NET1705_FService.Repositories.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? ApartmentId { get; set; }

    public int? ApartmentPackageId { get; set; }

    public string? UserName { get; set; }

    public DateTime? OrderDate { get; set; }

    public string PaymentMethod { get; set; }

    public string CustomerName { get; set; }

    public string Phone { get; set; }

    public DateTime? PaymentDate { get; set; }

    public double? TotalPrice { get; set; }

    public int PackageId { get; set; }

    public DateTime StartDate { get; set; }

    public bool? IsExtraOrder { get; set; }

    public int? ServiceId { get; set; }

    public string? TransactionNo { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Package? Package { get; set; }

    //public virtual ApartmentPackage ApartmentPackage { get; set; }

    public virtual ICollection<ApartmentPackage> ApartmentPackages { get; } = new List<ApartmentPackage>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}

public class OrderModel
{
    public int ApartmentId { get; set; }

    public int ApartmentPackageId { get; set; }

    [EmailAddress]
    public string? UserName { get; set; }

    public string PaymentMethod { get; set; }

    public string CustomerName { get; set; }

    [Phone]
    [StringLength(10)]
    public string Phone { get; set; }

    public DateTime? PaymentDate { get; set; }

    public int PackageId { get; set; }

    public DateTime StartDate { get; set; }

    public int ServiceId { get; set; }

    public string Type { get; set; }

    [Url]
    public string CallBackUrl { get; set; }

}

public class OrderViewModel
{
    public int Id { get; set; }

    public int? ApartmentId { get; set; }

    public int? ApartmentPackageId { get; set; }

    public string? UserName { get; set; }

    public DateTime? OrderDate { get; set; }

    public string PaymentMethod { get; set; }

    public string CustomerName { get; set; }

    public string Phone { get; set; }

    public DateTime? PaymentDate { get; set; }

    public double? TotalPrice { get; set; }

    public int PackageId { get; set; }

    public DateTime StartDate { get; set; }

    public bool? IsExtraOrder { get; set; }

    public int? ServiceId { get; set; }

    public string? TransactionNo { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual Package? Package { get; set; }

}
