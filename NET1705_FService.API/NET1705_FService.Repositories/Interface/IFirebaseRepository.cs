using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IFirebaseRepository
    {
        Task<string> PushNotificationFireBase(string title, string body, string accountId);
        Task<string> PushNotificationFireBaseToken(string title, string body, string token);
    }
}
