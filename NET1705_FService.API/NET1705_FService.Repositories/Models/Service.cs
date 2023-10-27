using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NET1705_FService.Repositories.Models;

public partial class Service
{
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public string UnsignName { get; set; }

    [Required(ErrorMessage = "Service's Name is required!")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Service's Description is required!")]
    [Display(Name = "Description")]
    public string Description { get; set; }
    public string Image { get; set; }

    [Required(ErrorMessage = "Service's Status is required!")]
    [Display(Name = "Status")]
    public bool Status { get; set; }

    public virtual ICollection<ApartmentPackageService> ApartmentPackageServices { get; } = new List<ApartmentPackageService>();

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();

    public virtual ICollection<PackageDetail> PackageDetails { get; } = new List<PackageDetail>();

    //public virtual ICollection<ServiceJoinStaff> ServiceJoinStaffs { get; } = new List<ServiceJoinStaff>();
}
