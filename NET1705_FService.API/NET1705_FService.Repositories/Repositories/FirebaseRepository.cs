using FServiceAPI.Repositories;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using Newtonsoft.Json.Linq;
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
            try
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
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        public async Task<string> PushNotificationFireBaseToken(string title, string body, string token)
        {
            try
            {
                return await FirebaseLibrary.SendMessageFireBase(title, body, token);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
