using FServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IAccountService
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<Accounts> GetAccountAsync(string id);
        public Task<ResponseModel> UpdateAccountAsync(string id, Accounts account);
        public Task<ResponseModel> DeleteAccountAsync(string id);
    }
}
