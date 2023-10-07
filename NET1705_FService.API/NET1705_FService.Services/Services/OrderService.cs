using NET1705_FService.Repositories.Data;
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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IPackageRepository _packageRepo;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;

        public OrderService(IOrderRepository repo, IPackageRepository packageRepo,
            IApartmentPackageRepository apartmentPackageRepo) 
        { 
            _repo = repo;
            _packageRepo = packageRepo;
            _apartmentPackageRepo = apartmentPackageRepo;
        }

        public async Task<ResponseModel> AddOrderAsync(OrderModel orderModel)
        {
            var package = await _packageRepo.GetPackageAsync(orderModel.PackageId);
            if (package == null)
            {
                return new ResponseModel { Status="Error", Message="Package was Not found."};
            }
            var result = await _repo.AddOrderAsync(orderModel);
            if (result == 0)
            {
                return new ResponseModel { Status = "Error", Message = "Somethings was error. Try again." };
            }
            return new ResponseModel { Status = "Success", Message = "Create order successfully" };
        }

        public async Task<ResponseModel> AddExtraOrderAsync(OrderModel extraModel)
        {
            var package = await _packageRepo.GetPackageAsync(extraModel.PackageId);
            if (package == null)
            {
                return new ResponseModel { Status = "Error", Message = "Package was Not found." };
            }
            var result = await _repo.AddExtraOrderAsync(extraModel);
            if (result == 0)
            {
                return new ResponseModel { Status = "Error", Message = "Somethings was error. Try again." };
            }
            return new ResponseModel { Status = "Success", Message = "Create order successfully" };
        }
    }
}
