using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string? AccountId {  get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Type { get; set; }

        public bool? IsRead { get; set; } = false;

        public string? Title { get; set; }

        public string? Action { get; set; }

        public string? Message { get; set; } = string.Empty;

        public int? ModelId { get; set; }

        public virtual Accounts Account { get; set; }
    }

    public class NotificationModel
    {
        public int Id { get; set; }

        public string? AccountId { get; set; }

        public DateTime? CreateDate { get; set; }

        public string? Type { get; set; }

        public bool? IsRead { get; set; } = false;

        public string? Title { get; set; }

        public string? Action { get; set; }

        public string? Message { get; set; } = string.Empty;

        public int? ModelId { get; set; }
    }
}
