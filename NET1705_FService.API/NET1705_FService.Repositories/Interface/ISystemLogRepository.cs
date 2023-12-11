using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface ISystemLogRepository
    {
        public Task<int> WriteLog(string message);
    }
}
