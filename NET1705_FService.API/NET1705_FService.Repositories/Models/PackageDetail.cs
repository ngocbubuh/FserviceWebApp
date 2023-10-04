using System;
using System.Collections.Generic;

namespace NET1705_FService.Repositories.Models;

public partial class PackageDetail
{
    public int Id { get; set; }

    public int? PackageId { get; set; }

    public int? ServiceId { get; set; }

    public int? Quantity { get; set; }

    public double? ExtraPrice { get; set; }

    public virtual Package Package { get; set; }

    public virtual Service Service { get; set; }
}
