using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Http;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using NET1715_FService.API.Repository.Inteface;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        private readonly IPackageRepository _packageRepo;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;
        private readonly IApartmentService _apartmentService;
        private readonly IServiceRepository _serviceRepo;
        private readonly IVnpayService _vnpayService;

        public OrderService(IOrderRepository repo, IPackageRepository packageRepo,
            IApartmentPackageRepository apartmentPackageRepo, IVnpayService vnpayService,
            IApartmentService apartmentService, IServiceRepository serviceRepo) 
        { 
            _repo = repo;
            _packageRepo = packageRepo;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apartmentService = apartmentService;
            _serviceRepo = serviceRepo;
            _vnpayService = vnpayService;
        }

        public async Task<ResponseModel> AddOrderAsync(OrderModel orderModel, HttpContext httpContext)
        {
            var apartment = await _apartmentService.GetApartmentByIdAsync(orderModel.ApartmentId);
            var typeId = apartment.TypeId;
            var package = await _packageRepo.GetPackageAsync(orderModel.PackageId, typeId);
            if (package == null)
            {
                return new ResponseModel { Status="Error", Message="Package was Not found."};
            }
            if (!await _apartmentPackageRepo.CheckApartmentPackagesByApartmentAndPackage(orderModel.ApartmentId, orderModel.PackageId))
            {
                return new ResponseModel { Status = "Error", Message = "Package is using." };
            }
            var result = await _repo.AddOrderAsync(orderModel);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Somethings was error. Try again." };
            }
            // payment
            var paymentURL = _vnpayService.CreatePaymentUrl(result, httpContext, package.UnsignName, orderModel.CallBackUrl);

            return new ResponseModel { Status = "Success", Message = "Create order successfully", PaymentUrl = paymentURL};
        }

        public async Task<ResponseModel> AddExtraOrderAsync(OrderModel extraModel, HttpContext httpContext)
        {
            var apmPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(extraModel.ApartmentPackageId);
            if (apmPackage == null)
            {
                return new ResponseModel { Status = "Error", Message = "Apartment Package was not found." };
            }
            var apartment = await _apartmentService.GetApartmentByIdAsync(extraModel.ApartmentId);
            var typeId = apartment.TypeId;
            var package = await _packageRepo.GetPackageAsync(extraModel.PackageId, typeId);
            if (package == null)
            {
                return new ResponseModel { Status = "Error", Message = "Package was not found." };
            }
            var buyService = package.PackageDetails.SingleOrDefault(p => p.ServiceId == extraModel.ServiceId);
            if (buyService == null)
            {
                return new ResponseModel { Status = "Error", Message = "Service was not found." };
            }
            var result = await _repo.AddExtraOrderAsync(extraModel);
            if (result == null)
            {
                return new ResponseModel { Status = "Error", Message = "Somethings was error. Try again." };
            }
            // payment
            var service = await _serviceRepo.GetServiceAsync(extraModel.ServiceId);
            var paymentURL = _vnpayService.CreatePaymentUrl(result, httpContext, service.UnsignName, extraModel.CallBackUrl);
            return new ResponseModel { Status = "Success", Message = "Create order successfully", PaymentUrl = paymentURL };
        }

        //public async Task<PagedList<Order>> GetOrdersByUserName(PaginationParameter paginationParameter,string userName)
        //{
        //    var orders = await _repo.GetOrderByUserNameAsync(paginationParameter ,userName);
        //    return orders;
        //}

        public async Task<PagedList<OrderViewModel>> GetOrdersByUserName(PaginationParameter paginationParameter, string userName)
        {
            var orders = await _repo.GetOrderByUserNameAsync(paginationParameter, userName);
            return orders;
        }

        public async Task<PagedList<Order>> GetAllOrdersAsync(PaginationParameter paginationParameter)
        {
            var orders = await _repo.GetAllOrdersAsync(paginationParameter);
            return orders;
        }

        public async Task<ResponseModel> UpdateOrderAsync(int id, Order order)
        {
            if (id != order.Id)
            {
                return new ResponseModel { Status = "Error", Message = "Id was not match"};
            }
            var dbOrder = await _repo.GetOrderByIdAsync(order.Id);
            if (dbOrder == null)
            {
                return new ResponseModel { Status = "Error", Message = "Order was not found." };
            }
            dbOrder.PaymentDate = order.PaymentDate;
            var updateOrderId = await _repo.UpdateOrderAsync(id, dbOrder);
            if (updateOrderId == 0)
            {
                return new ResponseModel { Status = "Error", Message = $"Can not update order id: ${id}" };
            }
            return new ResponseModel { Status = "Success", Message = updateOrderId.ToString() };
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _repo.GetOrderByIdAsync(id);
            return order;
        }
    }
}
