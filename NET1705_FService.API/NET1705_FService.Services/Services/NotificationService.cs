using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IUserService _userService;

        public NotificationService(INotificationRepository repo, IUserService userService) 
        { 
            _repo = repo;
            _userService = userService;
        }
        public async Task<int> AddNotificationByAccountId(string accountId, Notification notification)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return 0;
            }
            var account = await _userService.GetAccountAsync(accountId);
            if (account != null && notification != null)
            {
                await _repo.AddNotificationByAccountId(accountId, notification);
                return notification.Id;
            }
            return 0;

        }

        public async Task<PagedList<NotificationModel>> GetAllNotificationsByAccountIdAsync(PaginationParameter paginationParameter, string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return null;
            }
            var account = await _userService.GetAccountByUsernameAsync(userName);
            if (account != null)
            {
                return await _repo.GetAllNotificationsByAccountIdAsync(paginationParameter, account.Id);
            }
            return null;
        }

        public async Task<NotificationModel> GetNotificationById(int notificationId)
        {
            if (notificationId  == 0)
            {
                return null;
            }
            return await _repo.GetNotificationById(notificationId);
        }

        public async Task<int> MarkAllNotificationByAccountIdIsRead(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return 0;
            }
            var account = await _userService.GetAccountByUsernameAsync(userName);
            if (account != null)
            {
                return await _repo.MarkAllNotificationByAccountIdIsRead(account.Id);
            }
            return 0;
        }

        public async Task<int> MarkNotificationIsReadById(int notificationId)
        {
            if (notificationId == 0)
            {
                return 0;
            }
            return await _repo.MarkNotificationIsReadById(notificationId);
        }
    }
}
