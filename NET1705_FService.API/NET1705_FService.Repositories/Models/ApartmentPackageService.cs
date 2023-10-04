using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class ApartmentPackageService
{
    public int Id { get; set; }

    public int? ApartmentPackageId { get; set; }

    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }

    public int? UsedQuantity { get; set; }

    public int? RemainQuantity { get; set; }

    public bool? IsExtra { get; set; }

    public int? CountExtra { get; set; }

    public virtual ApartmentPackage ApartmentPackage { get; set; }

    public virtual Service Service { get; set; }
}
