using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Interface
{
    public interface INotificationService
    {
        public Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string userName);

        public Task<NotificationModel> GetNotificationById(int notificationId);

        public Task<int> AddNotificationByAccountId(string accountId, Notification notification);

        public Task<int> MarkAllNotificationByAccountIdIsRead(string userName);

        public Task<int> MarkNotificationIsReadById(int notificationId);
    }
}
