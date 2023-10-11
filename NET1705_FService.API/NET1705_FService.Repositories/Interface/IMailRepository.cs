using NET1705_FService.Repositories.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IMailRepository
    {
        public Task SendEmailAsync(MailRequest mailRequest);
    }
}
