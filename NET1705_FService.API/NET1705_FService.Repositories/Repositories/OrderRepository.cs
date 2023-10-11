using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        private readonly IPackageRepository _packageRepo;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;
        private readonly IApartPackageServiceRepository _apartPackageServiceRepo;
        private readonly IMapper _mapper;

        public OrderRepository(FserviceApiDatabaseContext context, IPackageRepository packageRepo,
            IApartmentPackageRepository apartmentPackageRepo, IApartPackageServiceRepository apartPackageServiceRepo,
            IMapper mapper)
        {
            _context = context;
            _packageRepo = packageRepo;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apartPackageServiceRepo = apartPackageServiceRepo;
            _mapper = mapper;
        }

        public async Task<int> AddOrderAsync(OrderModel orderModel)
        {
            if (_context == null)
            {
                return 0;
            }
            var newOrder = _mapper.Map<Order>(orderModel);
            newOrder.ApartmentPackageId = null;
            newOrder.IsExtraOrder = false;
            newOrder.ServiceId = null;

            var package = await _packageRepo.GetPackageAsync(newOrder.PackageId);

            newOrder.OrderDate = DateTime.Now;
            newOrder.TotalPrice = package.Price;

            _context.Add(newOrder);
            await _context.SaveChangesAsync();
            DateTime startDate = newOrder.StartDate;
            DateTime endDate = startDate;

            if (package.Duration == 4)
            {
                endDate = startDate.AddMonths(1);
            }
            else if (package.Duration == 1)
            {
                endDate = startDate.AddDays(7);
            }
            ApartmentPackage apartmentPackage = new ApartmentPackage
            {
                OrderId = newOrder.Id,
                ApartmentId = orderModel.ApartmentId,
                PackageId = orderModel.PackageId,
                StartDate = orderModel.StartDate,
                EndDate = endDate,
                PackageStatus = "Active",
            };
            int apmPackageId = await _apartmentPackageRepo.AddApartmentPackageAsync(apartmentPackage);
            // add apartment package service
            var packageDetails = package.PackageDetails;
            foreach (var service in packageDetails)
            {
                ApartmentPackageService apmService = new ApartmentPackageService
                {
                    ApartmentPackageId = apmPackageId,
                    ServiceId = service.ServiceId,
                    Quantity = service.Quantity,
                    UsedQuantity = 0,
                    RemainQuantity = service.Quantity,
                };
                await _apartPackageServiceRepo.AddApartPackageServiceAsync(apmService);
            }

            return newOrder.Id;
        }

        public async Task<int> AddExtraOrderAsync(OrderModel orderModel)
        {
            var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(orderModel.ApartmentPackageId);
            var extraOrder = _mapper.Map<Order>(orderModel);
            var package = await _packageRepo.GetPackageAsync(extraOrder.PackageId);
            var buyService = package.PackageDetails.SingleOrDefault(p => p.ServiceId == extraOrder.ServiceId);

            extraOrder.OrderDate = DateTime.Now;
            extraOrder.TotalPrice = buyService.ExtraPrice;
            extraOrder.IsExtraOrder = true;

            _context.Add(extraOrder);
            await _context.SaveChangesAsync();
            // update apartment package service
            var apartmentService = apartmentPackage.ApartmentPackageServices.SingleOrDefault(a => a.ServiceId == extraOrder.ServiceId);
            apartmentService.IsExtra = true;
            apartmentService.CountExtra = 1;
            await _apartPackageServiceRepo.UpdateApartPackageServiceAsync(apartmentService.Id ,apartmentService);

            return extraOrder.Id;
        }

        public async Task<PagedList<Order>> GetOrderByUserNameAsync(PaginationParameter paginationParameter, string userName)
        {
            if (_context == null)
            {
                return null;
            }
            var orders = await _context.Orders.Where(o => o.UserName.Equals(userName)).ToListAsync();

            return PagedList<Order>.ToPagedList(orders,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<PagedList<Order>> GetAllOrdersAsync(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var orders = _context.Orders.AsQueryable();
            if (!string.IsNullOrEmpty(paginationParameter.Search))
            {
                orders = orders.Where(o => o.UserName.Contains(paginationParameter.Search));
            }

            var allOrders = await orders.ToListAsync();

            return PagedList<Order>.ToPagedList(allOrders,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<int> UpdateOrderAsync(int id, Order order)
        {
            if (id == order.Id)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order.Id;
            }
            return 0;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            return order;
        }
    }
}
