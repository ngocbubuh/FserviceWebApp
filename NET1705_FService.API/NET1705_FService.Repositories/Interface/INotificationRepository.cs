using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface INotificationRepository
    {
        public Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string accountId);

        public Task<NotificationModel> GetNotificationById(int notificationId);

        public Task<int> AddNotificationByAccountId(string accountId, Notification notification);

        public Task<int> MarkAllNotificationByAccountIdIsRead(string accountId);

        public Task<int> MarkNotificationIsReadById(int notificationId);
    }
}
