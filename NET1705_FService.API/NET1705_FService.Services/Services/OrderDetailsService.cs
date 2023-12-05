using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IOrderDetailsRepository _repo;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;

        public OrderDetailsService(IOrderDetailsRepository repo, IApartmentPackageRepository apartmentPackageRepo) 
        { 
            _repo = repo;
            _apartmentPackageRepo = apartmentPackageRepo;
        }

        public async Task<ResponseModel> AddOrderDetails(UsingPackageModel usingPackage)
        {
            if (usingPackage.ApartmentPackageId <= 0 || usingPackage.ServiceId <= 0)
            {
                return new ResponseModel
                {
                    Status = "Error",
                    Message = "ApartmentPackageId or ServiceId is not exist"
                };
            }
            var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(usingPackage.ApartmentPackageId);
            if (apartmentPackage == null)
            {
                return new ResponseModel
                {
                    Status = "Error",
                    Message = $"Not found apartment package with Id = {usingPackage.ApartmentPackageId}"
                };
            }
            else
            {
                var apmPackageService = apartmentPackage.ApartmentPackageServices.FirstOrDefault(a => a.ServiceId == usingPackage.ServiceId);
                if (apmPackageService == null)
                {
                    return new ResponseModel
                    {
                        Status = "Error",
                        Message = $"Not found service with Id = {usingPackage.ServiceId}"
                    };
                }
            }
            var result = await _repo.AddOrderDetailsAsync(usingPackage);
            return result;
        }

        public Task<PagedList<OrderDetailsViewModel>> GetAllTaskForStaffAsync(PaginationParameter paginationParameter, string username)
        {
            var tasks = _repo.GetAllTaskForStaff(paginationParameter, username);
            return tasks;
        }

        public async Task<PagedList<OrderDetailsViewModel>> GetOrderDetailByApartmentPackageId(PaginationParameter paginationParameter, int apartmentPackageId)
        {
            var orderDetails = await _repo.GetOrderDetailByApartmentPackageId(paginationParameter ,apartmentPackageId);
            return orderDetails;
        }

        public async Task<OrderDetailsViewModel> GetOrderDetailsByIdAsync(int id)
        {
            var orderDetails = await _repo.GetOrderDetailByIdAsync(id);
            return orderDetails;
        }

        public async Task<int> UpdateTaskAsync(int id, OrderDetailModel orderDetailModel)
        {
            var result = await _repo.UpdateTaskAsync(id, orderDetailModel);
            return result;
        }

        public async Task<int> ConfirmStaffWork(int id, OrderDetailModel orderDetailModel)
        {
            var result = await _repo.ConfirmStaffWork(id, orderDetailModel);
            return result;
        }

        public async Task<int> CancelTaskAsync(int id, OrderDetailModel orderDetailModel)
        {
            return await _repo.CancelWork(id, orderDetailModel);
        }
    }
}
