﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class VnpayModel
    {
        public int vnp_Amount {  get; set; }
        public string vnp_PayDate { get; set; }
        public string vnp_ResponseCode { get; set; }
        public string vnp_TxnRef { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_TransactionStatus { get; set; }
        public string vnp_SecureHash { get; set; }
    }
}