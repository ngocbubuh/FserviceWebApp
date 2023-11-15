using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
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
        private readonly IApartmentRepository _apartmentRepo;
        private readonly IMapper _mapper;

        public OrderRepository(FserviceApiDatabaseContext context, IPackageRepository packageRepo,
            IApartmentPackageRepository apartmentPackageRepo, IApartPackageServiceRepository apartPackageServiceRepo,
            IApartmentRepository apartmentRepo,
            IMapper mapper)
        {
            _context = context;
            _packageRepo = packageRepo;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apartPackageServiceRepo = apartPackageServiceRepo;
            _apartmentRepo = apartmentRepo;
            _mapper = mapper;
        }

        public async Task<Order> AddOrderAsync(OrderModel orderModel)
        {
            if (_context == null)
            {
                return null;
            }
            var newOrder = _mapper.Map<Order>(orderModel);
            newOrder.ApartmentPackageId = null;
            newOrder.IsExtraOrder = false;
            newOrder.ServiceId = null;
            newOrder.PaymentDate = null;

            var apartment = await _apartmentRepo.GetApartmentByIdAsync(orderModel.ApartmentId);
            var typeId = apartment.TypeId;

            var package = await _packageRepo.GetPackageAsync(newOrder.PackageId, typeId);

            newOrder.OrderDate = DateTime.Now;
            newOrder.TotalPrice = package.PackagePrices.FirstOrDefault().Price;

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
                PackageStatus = "Disable",
            };

            // add apartment package service
            List<ApartmentPackageService> listServices = new List<ApartmentPackageService>();
            var packageDetails = package.PackageDetails;
            foreach (var service in packageDetails)
            {
                ApartmentPackageService apmService = new ApartmentPackageService
                {
                    ServiceId = service.ServiceId,
                    Quantity = service.Quantity,
                    UsedQuantity = 0,
                    RemainQuantity = service.Quantity,
                };
                listServices.Add(apmService);
            }
            if (listServices.Any())
            {
                apartmentPackage.ApartmentPackageServices = listServices;
            }

            int apmPackageId = await _apartmentPackageRepo.AddApartmentPackageAsync(apartmentPackage);
            //// add apartment package service
            //var packageDetails = package.PackageDetails;
            //foreach (var service in packageDetails)
            //{
            //    ApartmentPackageService apmService = new ApartmentPackageService
            //    {
            //        ApartmentPackageId = apmPackageId,
            //        ServiceId = service.ServiceId,
            //        Quantity = service.Quantity,
            //        UsedQuantity = 0,
            //        RemainQuantity = service.Quantity,
            //    };
            //    await _apartPackageServiceRepo.AddApartPackageServiceAsync(apmService);
            //}
            if (apmPackageId == 0)
            {
                return null;
            }
            return newOrder;
        }

        public async Task<Order> AddExtraOrderAsync(OrderModel orderModel)
        {
            var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(orderModel.ApartmentPackageId);
            var extraOrder = _mapper.Map<Order>(orderModel);
            var apartment = await _apartmentRepo.GetApartmentByIdAsync(orderModel.ApartmentId);
            var typeId = apartment.TypeId;
            var package = await _packageRepo.GetPackageAsync(extraOrder.PackageId, typeId);
            var buyService = package.PackageDetails.SingleOrDefault(p => p.ServiceId == extraOrder.ServiceId);

            extraOrder.OrderDate = DateTime.Now;
            extraOrder.TotalPrice = buyService.ExtraPrice;
            extraOrder.IsExtraOrder = true;
            extraOrder.PaymentDate = null;

            _context.Add(extraOrder);
            await _context.SaveChangesAsync();
            // update apartment package service
            var apartmentService = apartmentPackage.ApartmentPackageServices.SingleOrDefault(a => a.ServiceId == extraOrder.ServiceId);
            apartmentService.IsExtra = true;
            apartmentService.CountExtra = 0;
            await _apartPackageServiceRepo.UpdateApartPackageServiceAsync(apartmentService.Id, apartmentService);

            return extraOrder;
        }

        //public async Task<PagedList<Order>> GetOrderByUserNameAsync(PaginationParameter paginationParameter, string userName)
        //{
        //    if (_context == null)
        //    {
        //        return null;
        //    }
        //    var orders = await _context.Orders.Where(o => o.UserName == userName)
        //        .OrderByDescending(o => o.OrderDate).ToListAsync();

        //    return PagedList<Order>.ToPagedList(orders,
        //        paginationParameter.PageNumber,
        //        paginationParameter.PageSize);
        //}

        public async Task<PagedList<OrderViewModel>> GetOrderByUserNameAsync(PaginationParameter paginationParameter, string userName)
        {
            if (_context == null)
            {
                return null;
            }
            var orders = await _context.Orders.Where(o => o.UserName == userName)
                .OrderByDescending(o => o.OrderDate)
                .ProjectTo<OrderViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return PagedList<OrderViewModel>.ToPagedList(orders,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<PagedList<Order>> GetAllOrdersAsync(PaginationParameter paginationParameter)
        {
            if (_context == null)
            {
                return null;
            }
            var orders = _context.Orders.Include(o => o.Package).AsQueryable();
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
            var order = _context.Orders
                .Include(o => o.ApartmentPackages)
                .FirstOrDefault(o => o.Id == id);
            return order;
        }
    }
}
