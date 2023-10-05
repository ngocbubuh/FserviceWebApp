using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;   
        }
        public async Task<ResponseModel> DeleteAccountAsync(string id)
        {
            var deleteAccount = await _repo.GetAccountAsync(id);
            if (deleteAccount == null)
            {
                return new ResponseModel { Status = "Error", Message = $"Not found Package Id {id}" };
            }
            var result = await _repo.DeleteAccountAsync(id);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = $"Can not delete package {deleteAccount.Id}" };
            }
            return new ResponseModel { Status = "Success", Message = $"Delete successfully package {deleteAccount.Id}" };
        }

        public async Task<Accounts> GetAccountAsync(string id)
        {
            var account = await _repo.GetAccountAsync(id);
            return account;
        }

        public async Task<List<Accounts>> GetAllAccountAsync()
        {
            var accounts = await _repo.GetAllAccountAsync();
            return accounts;
        }

        public async Task<ResponseModel> UpdateAccountAsync(string id, Accounts account)
        {
            if (id == account.Id)
            {
                var result = await _repo.UpdateAccountAsync(id, account);
                if (result != null)
                {
                    return new ResponseModel { Status = "Success", Message = result.ToString() };
                }
                return new ResponseModel { Status = "Error", Message = "Update error." };
            }
            return new ResponseModel { Status = "Error", Message = "Id invalid" };
        }
    }
}
