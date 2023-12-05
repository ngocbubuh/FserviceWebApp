using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? ApartmentPackageId { get; set; }

        public int? ServiceId { get; set; }

        public int? Quantity { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public string? Note { get; set; }

        public string? ShiftTime { get; set; }

        public string StaffId { get; set; }

        public string Status { get; set; }

        public DateTime? WorkingDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public string? ReportImage { get; set;  }

        public bool? IsConfirm { get; set; }

        public string? Feedback { get; set; }

        public int? Rating { get; set; }

        public double? Amount { get; set; }

        public Service? Service { get; set; }

        public ApartmentPackage? ApartmentPackage { get; set; }
    }
}
