using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Models
{
    public partial class PackagePrice
    {
        public int Id { get; set; }

        public int PackageId { get; set; }

        public int TypeId { get; set; }

        public double Price { get; set; }

        public virtual Package? Package { get; set; }

        public virtual ApartmentType? Type { get; set; }
    }
}
