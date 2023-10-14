using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class ResponseModel
    {
        public string Status { get; set; }
        public string Message { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public System.Threading.Tasks.Task<String>? ConfirmEmailToken { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        public string? PaymentUrl { get; set; }
    }
}
