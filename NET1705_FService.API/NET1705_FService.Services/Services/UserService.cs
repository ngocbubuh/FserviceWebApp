﻿using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
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
            if (deleteAccount != null)
            {
                var result = await _repo.DeleteAccountAsync(id);
                if (result != null)
                {
                    return new ResponseModel { Status = "Success", Message = $"Delete successfully Account {deleteAccount.Id}!" };
                }
                return new ResponseModel { Status = "Error", Message = $"Cannot delete Account {deleteAccount.Id}, something went wrong!" };
            }
            return new ResponseModel { Status = "Error", Message = $"Not found Account Id {id}!" };
        }

        public async Task<Accounts> GetAccountAsync(string id)
        {
            var account = await _repo.GetAccountAsync(id);
            return account;
        }

        public async Task<AccountsModel> GetAccountByUsernameAsync(string name)
        {
            var account = await _repo.GetAccountByUsernameAsync(name);
            return account;
        }

        public async Task<PagedList<Accounts>> GetAccountsByRole(PaginationParameter paginationParameter, RoleModel role)
        {
            var accounts = await _repo.GetAccountsByRole(paginationParameter, role);
            return accounts;
        }

        public async Task<PagedList<Accounts>> GetAllAccountAsync(PaginationParameter paginationParameter)
        {
            var accounts = await _repo.GetAllAccountAsync(paginationParameter);
            return accounts;
        }

        public async Task<ResponseModel> UpdateAccountAsync(string id, AccountsModel accountUpdate)
        {
            var account = await _repo.GetAccountAsync(id);
            if (id == account.Id)
            {
                {
                    account.Name = accountUpdate.Name;
                    account.PhoneNumber = accountUpdate.PhoneNumber;
                    account.Address = accountUpdate.Address;
                    account.DateOfBirth = accountUpdate.DateOfBirth;
                    account.Avatar = accountUpdate.Avatar;
                }
                var result = await _repo.UpdateAccountAsync(id, account);
                if (result != null)
                {
                    return new ResponseModel { Status = "Success", Message = account.Email };
                }
                return new ResponseModel { Status = "Error", Message = "Update error!" };
            }
            return new ResponseModel { Status = "Error", Message = "Invalid Id!" };
        }
    }
}
