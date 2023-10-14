using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Identity;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;
        private readonly IUserRepository _userRepository;

        public AccountService(IAccountRepository repo, IUserRepository userRepository)
        {
            _repo = repo;
            _userRepository = userRepository;
        }

        public async Task<ResponseModel> ConfirmEmail(string token, string email)
        {
            var result = await _repo.ConfirmEmail(token, email);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }

        public async Task<AuthenticationResponseModel> SignInAsync(SignInModel model)
        {
            var result = await _userRepository.GetAccountByUsernameAsync(model.Email);
            if (result != null)
            {
                var response = await _repo.SignInAsync(model);
                return response;
            }
            return new AuthenticationResponseModel
            {
                Status = false,
                Message = "Username does not existed!",
                JwtToken = null,
                Expired = null
            };
        }

        //public async Task<ResponseModel> SignUpAdminAsync(SignUpModel model)
        //{
        //    var result = await _repo.SignUpAdminAsync(model);
        //    if (result == null)
        //    {
        //        return new ResponseModel { Status="Error", Message= "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
        //    }
        //    return result;
        //}

        public async Task<ResponseModel> SignUpAsync(SignUpModel model)
        {
            var result = await _repo.SignUpAsync(model);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }

        public async Task<ResponseModel> SignUpInternalAsync(SignUpModel model, RoleModel role)
        {
            var result = await _repo.SignUpInternalAsync(model, role);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
            }
            return result;
        }

        //public async Task<ResponseModel> SignUpStaffAsync(SignUpModel model)
        //{
        //    var result = await _repo.SignUpStaffAsync(model);
        //    if (result == null)
        //    {
        //        return new ResponseModel { Status = "Error", Message = "Oops! Our server is unable to fulfill this request at the moment! Please stand by!" };
        //    }
        //    return result;
        //}
    }
}
