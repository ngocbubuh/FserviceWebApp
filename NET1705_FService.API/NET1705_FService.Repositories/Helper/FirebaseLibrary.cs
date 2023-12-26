using FirebaseAdmin.Messaging;
using FServiceAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Helper
{
    public class FirebaseLibrary
    {
        public static async Task<string> SendMessageFireBase(string title, string body, string token)
        {
            var message = new Message()
            {
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                },
                Token = token
            };

            var reponse = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine($"Successfully: {reponse}");
            return reponse;
        }
    }
}
