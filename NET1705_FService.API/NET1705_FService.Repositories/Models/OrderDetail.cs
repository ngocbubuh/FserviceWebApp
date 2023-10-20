using NET1705_FService.Repositories.Data;
using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? ApartmentPackageId { get; set; }

    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? CustomerName { get; set; }

    public string? CustomerPhone { get; set; }

    public string? Note {  get; set; }

    public string? ShiftTime { get; set; }

    public string StaffId { get; set; }

    public string? Status { get; set; }

    public DateTime? CompleteDate { get; set; }

    public bool? IsConfirm { get; set; }

    public string? Feedback { get; set; }

    //public bool? IsExtraOrder { get; set; }

    public double? Amount { get; set; }

    public virtual ApartmentPackage ApartmentPackage { get; set; }

    public virtual Order Order { get; set; }

    public virtual Service Service { get; set; }

    public virtual Accounts Accounts { get; set; }
}

public class OrderDetailModel
{
    public int Id { get; set; }

    public TaskStatusModel Status { get; set; }

}
