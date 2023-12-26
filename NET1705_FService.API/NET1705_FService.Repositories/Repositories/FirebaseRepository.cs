using FServiceAPI.Repositories;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class FirebaseRepository : IFirebaseRepository
    {
        private readonly IUserRepository _userRepository;

        public FirebaseRepository(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<string> PushNotificationFireBase(string title, string body, string accountId)
        {
            var account = await _userRepository.GetAccountAsync(accountId);
            if (account != null) 
            { 
                if (account.DeviceToken != null)
                {
                    return await FirebaseLibrary.SendMessageFireBase(title, body, account.DeviceToken);
                }
            }
            return "";
        }

        public async Task<string> PushNotificationFireBaseToken(string title, string body, string token)
        {
            return await FirebaseLibrary.SendMessageFireBase(title, body, token);
        }
    }
}
