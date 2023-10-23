using FServiceAPI.Repositories;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _repo;
        private readonly IApartmentRepository _apartmentRepo;
        private readonly IAccountRepository _accountRepo;

        public ApartmentService(IApartmentRepository repo, IApartmentRepository apartmentRepo,
            IAccountRepository accountRepo)
        {
            _repo = repo;
            _apartmentRepo = apartmentRepo;
            _accountRepo = accountRepo;
        }
        public async Task<Apartment> GetApartmentByIdAsync(int id)
        {
            var apartment = await _repo.GetApartmentByIdAsync(id);
            return apartment;
        }

        public async Task<List<ApartmentModel>> GetApartmentsAsync(int? floorId, int? typeId, string? username)
        {
            var apartments = await _repo.GetApartmentsAsync(floorId, typeId, username);
            return apartments;
        }

        public async Task<List<ApartmentModel>> GetApartmentsByUserNameAsync(string userName)
        {
            var apartments = await _repo.GetApartmentsByUserNameAsync(userName);
            return apartments;
        }

        public async Task<ResponseModel> RegisApartment(int id, string userName)
        {
            var apartment = await _apartmentRepo.GetApartmentByIdAsync(id);
            if (apartment == null)
            {
                return new ResponseModel { Status = "Error", Message = "Apartment was not found." };
            }
            if (apartment.AccountId != null)
            {
                return new ResponseModel { Status = "Error", Message = "The apartment has been registered. Please contact the administrator to support." };
            }
            var account = await _accountRepo.GetAccountByUserName(userName);
            if (account == null)
            {
                return new ResponseModel { Status = "Error", Message = "Account is not exist." };
            }
            var result = await _apartmentRepo.RegisApartmentAsync(id, account.Id);
            if (result <= 0)
            {
                return new ResponseModel { Status = "Error", Message = $"Can not regis apartment for account: {userName}" };
            }
            return new ResponseModel { Status = "Success", Message = result.ToString() };
        }
    }
}
