using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class UsingPackageModel
    {
        public int ApartmentPackageId { get; set; }
        public int ServiceId { get; set; }
        public string CustomerName { get; set; }
        [Phone]
        public string CustomerPhone { get; set; }
        public string Note {  get; set; }
        public ShiftTimeModel ShiftTime { get; set; }
    }
}
