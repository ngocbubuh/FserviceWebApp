using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Services.Interface;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _repo;

        public MailService(IMailRepository repo)
        {
            _repo = repo;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            await _repo.SendEmailAsync(mailRequest);
        }
    }
}
