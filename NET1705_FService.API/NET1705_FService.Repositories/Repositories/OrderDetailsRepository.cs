﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FServiceAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Repositories
{
    public class OrderDetailsRepository : IOrderDetailsRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;
        private readonly IApartPackageServiceRepository _apmPackageServiceRepo;
        private readonly IPackageRepository _packageRepo;
        private readonly IAccountRepository _accountRepo;
        private readonly IMapper _mapper;

        public OrderDetailsRepository(FserviceApiDatabaseContext context, IApartmentPackageRepository apartmentPackageRepo,
            IApartPackageServiceRepository apmPackageServiceRepo, IPackageRepository packageRepo,
            IAccountRepository accountRepo, IMapper mapper)
        {
            _context = context;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apmPackageServiceRepo = apmPackageServiceRepo;
            _packageRepo = packageRepo;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public async Task<OrderDetailsViewModel> GetOrderDetailByIdAsync(int id)
        {
            var orderDetails = _context.OrderDetails.FirstOrDefault(o => o.Id == id);
            var orderDetailsView = _mapper.Map<OrderDetailsViewModel>(orderDetails);
            return orderDetailsView;
        }

        public async Task<ResponseModel> AddOrderDetailsAsync(UsingPackageModel usingPackage)
        {
            var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(usingPackage.ApartmentPackageId);
            var typeId = apartmentPackage.Apartment.TypeId;
            var apmPackageService = apartmentPackage.ApartmentPackageServices.FirstOrDefault(a => a.ServiceId == usingPackage.ServiceId);
            //var apmPackageServices = apartmentPackage.ApartmentPackageServices;
            if (apartmentPackage == null || !apartmentPackage.PackageStatus.Equals("Active"))
            {
                return new ResponseModel { Status = "Error", Message = "This apartment package was expired." };
            }
            if (apmPackageService.RemainQuantity == 0)
            {
                if (apmPackageService.IsExtra == true && apmPackageService.CountExtra > 0)
                {
                    var usePackage = await _packageRepo.GetPackageAsync(apartmentPackage.PackageId, typeId);
                    var priceService = usePackage.PackageDetails.SingleOrDefault(p => p.ServiceId == usingPackage.ServiceId).ExtraPrice;
                    var staffWorkExtra = await AssignStaff();
                    OrderDetail orderDetailExtra = _mapper.Map<OrderDetail>(usingPackage);
                    orderDetailExtra.OrderId = apartmentPackage.OrderId;
                    orderDetailExtra.Quantity = 1;
                    orderDetailExtra.CreatedDate = DateTime.Now;
                    orderDetailExtra.Amount = priceService;
                    orderDetailExtra.StaffId = staffWorkExtra.Id;
                    orderDetailExtra.ShiftTime = GetShiftTimeAsString(usingPackage.ShiftTime);
                    orderDetailExtra.Status = "Pending";

                    //OrderDetail orderDetailExtra = new OrderDetail()
                    //{
                    //    OrderId = apartmentPackage.OrderId,
                    //    ApartmentPackageId = usingPackage.ApartmentPackageId,
                    //    ServiceId = usingPackage.ServiceId,
                    //    Quantity = 1,
                    //    CreatedDate = DateTime.Now,
                    //    CustomerName = usingPackage.CustomerName,
                    //    CustomerPhone = usingPackage.CustomerPhone,
                    //    Amount = priceService,
                    //    // assign staff
                    //    StaffId = staffWorkExtra.Id
                    //};
                    _context.Add(orderDetailExtra);
                    await _context.SaveChangesAsync();
                    // update quantity
                    apmPackageService.CountExtra -= 1;
                    int updateQuantityExtra = await _apmPackageServiceRepo.UpdateApartPackageServiceAsync(usingPackage.ServiceId, apmPackageService);
                    if (updateQuantityExtra == 0)
                    {
                        return new ResponseModel { Status = "Error", Message = "It is issue when update quantity service" };
                    }
                    return new ResponseModel
                    {
                        Status = "Success",
                        Message = orderDetailExtra.Id.ToString()
                    };
                }
                return new ResponseModel { Status = "Error", Message = "Not enough quantity avaiable of service" };
            }

            //----------------

            var staffWork = await AssignStaff();
            OrderDetail orderDetail = _mapper.Map<OrderDetail>(usingPackage);
            orderDetail.OrderId = apartmentPackage.OrderId;
            orderDetail.Quantity = 1;
            orderDetail.CreatedDate = DateTime.Now;
            orderDetail.Amount = 0;
            orderDetail.StaffId = staffWork.Id;
            orderDetail.ShiftTime = GetShiftTimeAsString(usingPackage.ShiftTime);
            orderDetail.Status = "Pending";

            //OrderDetail orderDetail = new OrderDetail()
            //{
            //    OrderId = apartmentPackage.OrderId,
            //    ApartmentPackageId = usingPackage.ApartmentPackageId,
            //    ServiceId = usingPackage.ServiceId,
            //    Quantity = 1,
            //    CreatedDate = DateTime.Now,
            //    CustomerName = usingPackage.CustomerName,
            //    CustomerPhone = usingPackage.CustomerPhone,
            //    Amount = 0,
            //    // assign staff
            //    StaffId = staffWork.Id
            //};

            _context.Add(orderDetail);
            await _context.SaveChangesAsync();
            // update quantity
            apmPackageService.UsedQuantity += 1;
            apmPackageService.RemainQuantity -= 1;
            int updateQuantity = await _apmPackageServiceRepo.UpdateApartPackageServiceAsync(usingPackage.ServiceId, apmPackageService);
            if (updateQuantity == 0)
            {
                return new ResponseModel { Status = "Error", Message = "It is issue when update quantity service" };
            }
            return new ResponseModel
            {
                Status = "Success",
                Message = orderDetail.Id.ToString()
            };
        }

        public async Task<PagedList<OrderDetailsViewModel>> GetAllTaskForStaff(PaginationParameter paginationParameter, string staffId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(o => o.StaffId == staffId)
                .Include(o => o.Service)
                .OrderByDescending(o => o.CreatedDate)
                .ProjectTo<OrderDetailsViewModel>(_mapper.ConfigurationProvider) // Use ProjectTo to map to ViewModel
                .ToListAsync();


            //var tasks = _mapper.Map<OrderDetailsViewModel>(orderDetails);

            return PagedList<OrderDetailsViewModel>.ToPagedList(orderDetails,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<int> UpdateTaskAsync(int id, OrderDetailModel orderDetailModel)
        {
            if (id == orderDetailModel.Id)
            {
                var updateOrder = _context.OrderDetails.FirstOrDefault(o => o.Id == id);
                if (updateOrder != null)
                {
                    if (updateOrder.Status.Trim().Equals("Pending"))
                    {
                        updateOrder.Status = orderDetailModel.Status.ToString();
                    }
                    if (updateOrder.Status.Trim().Equals("Working"))
                    {
                        updateOrder.Status = orderDetailModel.Status.ToString();
                        updateOrder.CompleteDate = DateTime.Now;
                    }
                    _context.OrderDetails!.Update(updateOrder);
                    await _context.SaveChangesAsync();
                }
                return updateOrder.Id;
            }
            return 0;
        }

        //tools
        private async Task<Accounts> AssignStaff()
        {
            var staffs = await _accountRepo.GetAllStaffsAsync();
            Accounts workStaff = null;
            foreach (var staff in staffs)
            {
                // staff chua co job trong ngay
                var orderDetails = await _context.OrderDetails
                    .Where(o => o.StaffId == staff.Id)
                    .Where(o => o.CreatedDate.Date == DateTime.Now.Date)
                    .ToListAsync();
                if (!orderDetails.Any())
                {
                    workStaff = staff;
                    break;
                }

            }
            // tat ca staff da co job trong ngay
            if (workStaff == null)
            {
                var staffIdWithMinRecords = _context.OrderDetails
                    .Where(o => o.CreatedDate.Date == DateTime.Now.Date)
                    .GroupBy(o => o.StaffId)
                    .Select(group => new
                    {
                        StaffId = group.Key,
                        RecordCount = group.Count()
                    })
                    .OrderBy(group => group.RecordCount)
                    .ThenBy(group => group.StaffId)
                    .Select(group => group.StaffId)
                    .FirstOrDefault();

                workStaff = await _context.Accounts.FindAsync(staffIdWithMinRecords);
            }
            return workStaff;
        }

        public string GetShiftTimeAsString(ShiftTimeModel shiftTime)
        {
            switch (shiftTime)
            {
                case ShiftTimeModel.SevenToNineAM:
                    return "7:00AM - 9:00AM";
                case ShiftTimeModel.NineToElevenAM:
                    return "9:00AM - 11:00AM";
                case ShiftTimeModel.OneToThreePM:
                    return "1:00PM - 3:00PM";
                case ShiftTimeModel.ThreeToFivePM:
                    return "3:00PM - 5:00PM";
                default:
                    return "Unknown";
            }
        }


    }
}
