using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Interface
{
    public interface IUserService
    {
        public Task<Accounts> GetAccountAsync(string id);
        public Task<PagedList<Accounts>> GetAllAccountAsync(PaginationParameter paginationParameter);
        public Task<ResponseModel> UpdateAccountAsync(string id, Accounts account);
        public Task<ResponseModel> DeleteAccountAsync(string id);
    }
}
