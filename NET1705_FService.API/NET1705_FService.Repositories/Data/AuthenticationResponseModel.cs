using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class AuthenticationResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string JwtToken { get; set; }
        public DateTime? Expired { get; set; }
    }
}
