using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Models
{
    public class SystemLog
    {
        public int Id {  get; set; }

        public DateTime CreateDate { get; set; }

        public string Message { get; set; } = "";
    }
}
