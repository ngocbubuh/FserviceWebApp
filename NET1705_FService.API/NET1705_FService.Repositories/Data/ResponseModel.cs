using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public string? PaymentUrl { get; set; }
    }
}
