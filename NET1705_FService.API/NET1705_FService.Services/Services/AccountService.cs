using FServiceAPI.Models;
using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
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

        public async Task<string> SignInAsync(SignInModel model)
        {
            var token = await _repo.SignInAsync(model);
            if (token == null) 
            {
                return null;
            }
            return token;
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var result = await _repo.SignUpAsync(model);
            if (result == null)
            {
                return null;
            }
            return result;
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
