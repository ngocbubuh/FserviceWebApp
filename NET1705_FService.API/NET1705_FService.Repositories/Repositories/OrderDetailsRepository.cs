using AutoMapper;
using AutoMapper.QueryableExtensions;
using FServiceAPI.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
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
        private readonly INotificationRepository _notificationRepo;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepo;
        private readonly IMapper _mapper;

        public OrderDetailsRepository(FserviceApiDatabaseContext context, IApartmentPackageRepository apartmentPackageRepo,
            IApartPackageServiceRepository apmPackageServiceRepo, IPackageRepository packageRepo,
            IAccountRepository accountRepo, IMapper mapper, INotificationRepository notificationRepo,
            IOrderRepository orderRepository, IUserRepository userRepository,
            IServiceRepository serviceRepo)
        {
            _context = context;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apmPackageServiceRepo = apmPackageServiceRepo;
            _packageRepo = packageRepo;
            _accountRepo = accountRepo;
            _notificationRepo = notificationRepo;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _serviceRepo = serviceRepo;
            _mapper = mapper;
        }

        public async Task<OrderDetailsViewModel> GetOrderDetailByIdAsync(int id)
        {
            var orderDetails = _context.OrderDetails.Include(o => o.Service).FirstOrDefault(o => o.Id == id);
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
            if (staffWork == null)
            {
                return new ResponseModel { Status = "Error", Message = "Staffs is not avalable." };
            }
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
            // send noti

            var order = await _orderRepository.GetOrderByIdAsync(orderDetail.OrderId.Value);
            if (order != null)
            {
                var customer = await _userRepository.GetAccountByUsernameAsync(order.UserName);
                var usingService = await _serviceRepo.GetServiceAsync(usingPackage.ServiceId);

                // send noti to customer
                if (customer != null && usingService != null)
                {
                    Notification notification = new Notification
                    {
                        AccountId = customer.Id,
                        CreateDate = DateTime.Now,
                        Type = NotificationType.Service.ToString(),
                        Title = usingService.Name,
                        Action = "Đã có nhân viên",
                        Message = $"Nhân viên {staffWork.Name} sẽ sớm liên hệ với bạn. Hãy nhớ chú ý điện thoại!",
                        ModelId = apartmentPackage.Id
                    };
                    await _notificationRepo.AddNotificationByAccountId(customer.Id, notification);
                }

                // send noti to staff
                Notification notificationStaff = new Notification
                {
                    AccountId = staffWork.Id,
                    CreateDate = DateTime.Now,
                    Type = NotificationType.Service.ToString(),
                    Title = usingService.Name,
                    Action = "Công việc mới",
                    Message = $"Công việc mới của bạn đã được lên lịch làm việc, hãy kiểm tra bảng công việc của mình.",
                    ModelId = orderDetail.Id
                };
                await _notificationRepo.AddNotificationByAccountId(customer.Id, notificationStaff);
            }

            return new ResponseModel
            {
                Status = "Success",
                Message = orderDetail.Id.ToString()
            };
        }

        public async Task<PagedList<OrderDetailsViewModel>> GetAllTaskForStaff(PaginationParameter paginationParameter, string username)
        {
            var staff = await _accountRepo.GetAccountByUserName(username);
            if (staff == null)
            {
                return null;
            }
            var orderDetails = await _context.OrderDetails
                .Where(o => o.StaffId == staff.Id)
                .Include(o => o.Service)
                .Include(a => a.ApartmentPackage)
                .OrderByDescending(o => o.CreatedDate)
                .ProjectTo<OrderDetailsViewModel>(_mapper.ConfigurationProvider)
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
                var updateOrderDetail = _context.OrderDetails.FirstOrDefault(o => o.Id == id);
                if (updateOrderDetail != null)
                {
                    var usingService = await _serviceRepo.GetServiceAsync(updateOrderDetail.ServiceId.Value);
                    var order = await _orderRepository.GetOrderByIdAsync(updateOrderDetail.OrderId.Value);
                    if (updateOrderDetail.Status.Trim() == TaskStatusModel.Pending.ToString())
                    {
                        updateOrderDetail.WorkingDate = DateTime.Now;
                        updateOrderDetail.Status = orderDetailModel.Status.ToString();

                        // send noti to staff
                        if (usingService != null)
                        {
                            Notification notification = new Notification
                            {
                                AccountId = updateOrderDetail.StaffId,
                                CreateDate = DateTime.Now,
                                Type = NotificationType.Service.ToString(),
                                Title = usingService.Name,
                                Action = "Trạng thái",
                                Message = $"Chuẩn bị đến nhà khách hàng làm việc trong khoảng {updateOrderDetail.ShiftTime}",
                                ModelId = updateOrderDetail.Id
                            };
                            await _notificationRepo.AddNotificationByAccountId(updateOrderDetail.StaffId, notification);
                        }

                        // send noti to customer
                        if (order != null)
                        {
                            var customer = await _userRepository.GetAccountByUsernameAsync(order.UserName);
                            if (customer != null && usingService != null)
                            {
                                Notification notification = new Notification
                                {
                                    AccountId = customer.Id,
                                    CreateDate = DateTime.Now,
                                    Type = NotificationType.Service.ToString(),
                                    Title = usingService.Name,
                                    Action = "Trạng thái",
                                    Message = $"Nhân viên đang sắp đến.",
                                    ModelId = updateOrderDetail.ApartmentPackageId
                                };
                                await _notificationRepo.AddNotificationByAccountId(customer.Id, notification);
                            }
                        }

                    }
                    else if (updateOrderDetail.Status.Trim() == TaskStatusModel.Working.ToString())
                    {
                        updateOrderDetail.Status = orderDetailModel.Status.ToString();
                        updateOrderDetail.CompleteDate = DateTime.Now;
                        // update image
                        if (!string.IsNullOrEmpty(updateOrderDetail.ReportImage))
                        {
                            updateOrderDetail.ReportImage = orderDetailModel.ReportImage;
                        }
                        // send noti to staff
                        if (usingService != null)
                        {
                            Notification notification = new Notification
                            {
                                AccountId = updateOrderDetail.StaffId,
                                CreateDate = DateTime.Now,
                                Type = NotificationType.Service.ToString(),
                                Title = usingService.Name,
                                Action = "Hoàn thành",
                                Message = $"Bạn đã hoàn thành công việc của mình. Hãy chờ khách hàng đánh giá!",
                                ModelId = updateOrderDetail.Id
                            };
                            await _notificationRepo.AddNotificationByAccountId(updateOrderDetail.StaffId, notification);
                        }

                        // send noti to customer
                        if (order != null)
                        {
                            var customer = await _userRepository.GetAccountByUsernameAsync(order.UserName);
                            if (customer != null && usingService != null)
                            {
                                Notification notification = new Notification
                                {
                                    AccountId = customer.Id,
                                    CreateDate = DateTime.Now,
                                    Type = NotificationType.Service.ToString(),
                                    Title = usingService.Name,
                                    Action = "Trạng thái",
                                    Message = $"Nhân viên đã hoàn thành công việc. Đánh giá ngay!",
                                    ModelId = updateOrderDetail.ApartmentPackageId
                                };
                                await _notificationRepo.AddNotificationByAccountId(customer.Id, notification);
                            }
                        }
                    }
                    _context.OrderDetails!.Update(updateOrderDetail);
                    await _context.SaveChangesAsync();

                    
                }
                return updateOrderDetail.Id;
            }
            return 0;
        }

        public async Task<int> ConfirmStaffWork(int id, OrderDetailModel orderDetailModel)
        {
            if (id == orderDetailModel.Id)
            {
                var updateOrder = _context.OrderDetails.FirstOrDefault(o => o.Id == id);
                if (updateOrder != null)
                {
                    // user confirm work when staff make completed
                    if (updateOrder.Status.Trim() == TaskStatusModel.Completed.ToString())
                    {
                        if (orderDetailModel.IsConfirm != null)
                        {
                            updateOrder.IsConfirm = orderDetailModel.IsConfirm;
                        }
                        if (!string.IsNullOrEmpty(orderDetailModel.Feedback))
                        {
                            updateOrder.Feedback = orderDetailModel.Feedback;
                        }
                        updateOrder.Rating = orderDetailModel.Rating;
                    }
                    _context.OrderDetails!.Update(updateOrder);
                    await _context.SaveChangesAsync();
                }
                return updateOrder.Id;
            }
            return 0;
        }

        public async Task<PagedList<OrderDetailsViewModel>> GetOrderDetailByApartmentPackageId(PaginationParameter paginationParameter,
            int apartmentPackageId)
        {
            var orderDetails = await _context.OrderDetails
                .Where(o => o.ApartmentPackageId == apartmentPackageId)
                .Include(o => o.Service)
                .OrderByDescending(o => o.CreatedDate)
                .ProjectTo<OrderDetailsViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();


            //var tasks = _mapper.Map<OrderDetailsViewModel>(orderDetails);

            return PagedList<OrderDetailsViewModel>.ToPagedList(orderDetails,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<int> CancelWork(int id, OrderDetailModel orderDetailModel)
        {
            if (id == orderDetailModel.Id)
            {
                var work = await _context.OrderDetails.FirstOrDefaultAsync(o => o.Id == id);
                if (work != null && work.Status == TaskStatusModel.Pending.ToString())
                {
                    // re-assign anothers staff
                    var workStaff = await ReAssignStaff(work.StaffId);
                    var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(work.ApartmentPackageId.Value);
                    if (workStaff == null)
                    {
                        // refund number of remain quantity service for customer
                        
                        if (apartmentPackage != null)
                        {
                            foreach (var item in apartmentPackage.ApartmentPackageServices)
                            {
                                if (item.ServiceId == work.ServiceId)
                                {
                                    item.UsedQuantity -= 1;
                                    item.RemainQuantity += 1;
                                    break;
                                }
                            }
                            int updateQuantity = await _apartmentPackageRepo.UpdateApartmentPackageAsync(apartmentPackage.Id, apartmentPackage);

                            work.Status = orderDetailModel.Status.ToString();
                            _context.OrderDetails!.Update(work);
                            await _context.SaveChangesAsync();

                            if (updateQuantity != 0)
                            {
                                var order = await _orderRepository.GetOrderByIdAsync(work.Id);
                                if (order != null)
                                {
                                    var customer = await _userRepository.GetAccountByUsernameAsync(order.UserName);
                                    var usingService = await _serviceRepo.GetServiceAsync(work.ServiceId.Value);
                                    if (customer != null && usingService != null)
                                    {
                                        Notification notification = new Notification
                                        {
                                            AccountId = customer.Id,
                                            CreateDate = DateTime.Now,
                                            Type = NotificationType.Service.ToString(),
                                            Title = usingService.Name,
                                            Action = "Huỷ lịch",
                                            Message = $"Nhân viên đã huỷ lịch làm việc, bạn vui lòng đặt lại lịch khác. Xin cảm ơn!",
                                            ModelId = apartmentPackage.Id
                                        };
                                        await _notificationRepo.AddNotificationByAccountId(customer.Id, notification);
                                    }
                                }
                                return work.Id;
                            }
                        }
                    } 
                    else
                    {
                        // update work for staff re-assign
                        work.StaffId = workStaff.Id;
                        work.Status = TaskStatusModel.Pending.ToString();
                        _context.OrderDetails!.Update(work);
                        await _context.SaveChangesAsync();

                        // send noti to staff and customer
                        var order = await _orderRepository.GetOrderByIdAsync(work.Id);
                        if (order != null)
                        {
                            var customer = await _userRepository.GetAccountByUsernameAsync(order.UserName);
                            var usingService = await _serviceRepo.GetServiceAsync(work.ServiceId.Value);
                            if (customer != null && usingService != null)
                            {
                                Notification notificationStaff = new Notification
                                {
                                    AccountId = work.StaffId,
                                    CreateDate = DateTime.Now,
                                    Type = NotificationType.Service.ToString(),
                                    Title = usingService.Name,
                                    Action = "Trạng thái",
                                    Message = $"Chuẩn bị đến nhà khách hàng làm việc trong khoảng {work.ShiftTime}",
                                    ModelId = work.Id
                                };
                                await _notificationRepo.AddNotificationByAccountId(work.StaffId, notificationStaff);

                                Notification notification = new Notification
                                {
                                    AccountId = customer.Id,
                                    CreateDate = DateTime.Now,
                                    Type = NotificationType.Service.ToString(),
                                    Title = usingService.Name,
                                    Action = "Đã có nhân viên mới",
                                    Message = $"Nhân viên {workStaff.Name} đã tiếp nhận công việc và sẽ sớm liên hệ với bạn. Hãy nhớ chú ý điện thoại!",
                                    ModelId = apartmentPackage.Id
                                };
                                await _notificationRepo.AddNotificationByAccountId(customer.Id, notification);
                            }
                        }
                        return work.Id;
                    }
                }
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
                //var staffIdWithMinRecords = _context.OrderDetails
                //    .Where(o => o.CreatedDate.Date == DateTime.Now.Date)
                //    .GroupBy(o => o.StaffId)
                //    .Select(group => new
                //    {
                //        StaffId = group.Key,
                //        RecordCount = group.Count()
                //    })
                //    .OrderBy(group => group.RecordCount)
                //    .ThenBy(group => group.StaffId)
                //    .Select(group => group.StaffId)
                //    .FirstOrDefault();

                //workStaff = await _context.Accounts.FindAsync(staffIdWithMinRecords);
                var staffIdWithMinRecords = staffs
                    .Select(staff => new
                    {
                        StaffId = staff.Id,
                        RecordCount = _context.OrderDetails
                            .Count(o => o.StaffId == staff.Id && o.CreatedDate.Date == DateTime.Now.Date)
                    })
                    .OrderBy(group => group.RecordCount)
                    .ThenBy(group => group.StaffId)
                    .Select(group => group.StaffId)
                    .FirstOrDefault();

                // Retrieve the staff member with the minimum jobs using their ID
                workStaff = staffs.FirstOrDefault(staff => staff.Id == staffIdWithMinRecords);
            }
            return workStaff;
        }

        private async Task<Accounts> ReAssignStaff(string cancelStaffId)
        {
            var staffs = await _accountRepo.GetAllStaffsAsync();
            Accounts workStaff = null;
            foreach (var staff in staffs)
            {
                // Skip the staff member with the specified ID
                if (staff.Id == cancelStaffId)
                {
                    continue;
                }

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
                
                var staffIdWithMinRecords = staffs
                    .Where(staff => staff.Id != cancelStaffId)
                    .Select(staff => new
                    {
                        StaffId = staff.Id,
                        RecordCount = _context.OrderDetails
                            .Count(o => o.StaffId == staff.Id && o.CreatedDate.Date == DateTime.Now.Date)
                    })
                    .OrderBy(group => group.RecordCount)
                    .ThenBy(group => group.StaffId)
                    .Select(group => group.StaffId)
                    .FirstOrDefault();

                // Retrieve the staff member with the minimum jobs using their ID
                workStaff = staffs.FirstOrDefault(staff => staff.Id == staffIdWithMinRecords);
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
