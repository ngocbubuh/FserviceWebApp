using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Data
{
    public class VnpayModel
    {
        //public int vnp_Amount {  get; set; }
        //public string vnp_PayDate { get; set; }
        //public string vnp_ResponseCode { get; set; }
        //public string vnp_TxnRef { get; set; }
        //[MaxLength(15)]
        //public string vnp_TransactionNo { get; set; }
        //public string vnp_TransactionStatus { get; set; }
        //public string vnp_SecureHash { get; set; }
        public string vnp_TmnCode { get; set; } = string.Empty;
        public string vnp_BankCode { get; set; } = string.Empty;
        public string vnp_BankTranNo { get; set; } = string.Empty;
        public string vnp_CardType { get; set; } = string.Empty;
        public string vnp_OrderInfo { get; set; } = string.Empty;
        public string vnp_TransactionNo { get; set; } = string.Empty;
        public string vnp_TransactionStatus { get; set; } = string.Empty;
        public string vnp_TxnRef { get; set; } = string.Empty;
        public string vnp_SecureHashType { get; set; } = string.Empty;
        public string vnp_SecureHash { get; set; } = string.Empty;
        public int? vnp_Amount { get; set; }
        public string? vnp_ResponseCode { get; set; }
        public string vnp_PayDate { get; set; } = string.Empty;
    }
}
