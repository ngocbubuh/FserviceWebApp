using FServiceAPI.Models;
using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> SignUpAsync(SignUpModel model);
        public Task<string> SignInAsync(SignInModel model);
        public Task<Accounts> GetAccountAsync(string id);
        public Task<string> UpdateAccountAsync(string id, Accounts account);
        public Task<string> DeleteAccountAsync(string id);
    }
}
