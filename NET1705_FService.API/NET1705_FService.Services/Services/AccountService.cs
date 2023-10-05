using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Data;
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

        public async Task<string> SignInAsync(SignInModel model)
        {
            var token = await _repo.SignInAsync(model);
            if (token == null) 
            {
                return "Error";
            }
            return token;
        }

        public async Task<ResponseModel> SignUpAdminAsync(SignUpModel model)
        {
            var result = await _repo.SignUpAdminAsync(model);
            if (result == null)
            {
                return new ResponseModel { Status="Error", Message= "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }

        public async Task<ResponseModel> SignUpAsync(SignUpModel model)
        {
            var result = await _repo.SignUpAsync(model);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }

        public async Task<ResponseModel> SignUpStaffAsync(SignUpModel model)
        {
            var result = await _repo.SignUpStaffAsync(model);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }
    }
}
