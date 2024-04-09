using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class SystemLogRepository : ISystemLogRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public SystemLogRepository(FserviceApiDatabaseContext context) 
        {
            _context = context;
        }
        public async Task<int> WriteLog(string message)
        {
            try
            {
                if (_context == null)
                {
                    return 0;
                }

                SystemLog log = new SystemLog
                {
                    CreateDate = DateTime.Now,
                    Message = message
                };

                _context.SystemLogs.Add(log);
                await _context.SaveChangesAsync();
                return log.Id;
            }
            catch
            {
                return 0;
            }
            
        }
    }
}
