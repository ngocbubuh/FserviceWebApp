using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Interface
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailRequest mailRequest);
    }
}
