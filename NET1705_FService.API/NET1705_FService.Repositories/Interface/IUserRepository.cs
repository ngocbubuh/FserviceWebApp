using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IUserRepository
    {
        public Task<List<Accounts>> GetAllAccountAsync();
        public Task<Accounts> GetAccountAsync(string id);
        public Task<string> UpdateAccountAsync(string id, Accounts account);
        public Task<string> DeleteAccountAsync(string id);
    }
}
