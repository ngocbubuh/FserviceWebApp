using NET1705_FService.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NET1705_FService.Services.Services
{
    public class VnpayService : IVnpayService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IApartmentPackageRepository _apartmentPackageRepo;
        private readonly IApartPackageServiceRepository _apartPackageServiceRepo;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public VnpayService(IOrderRepository orderRepository, IApartmentPackageRepository apartmentPackageRepo,
            IApartPackageServiceRepository apartPackageServiceRepo, INotificationService notificationService,
            IUserService userService)
        {
            _orderRepository = orderRepository;
            _apartmentPackageRepo = apartmentPackageRepo;
            _apartPackageServiceRepo = apartPackageServiceRepo;
            _notificationService = notificationService;
            _userService = userService;
        }
        public string CreatePaymentUrl(Order model, HttpContext context, string vnp_OrderInfo, string callBackUrl)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            //var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            DateTime timeNow = DateTime.Now;
            //var tick = DateTime.Now.Ticks.ToString();
            var orderId = model.Id.ToString();
            // check payment call back
            //var acceptedUrls = _configuration["AcceptPaymentUrl:Url"];
            //string[] urls = acceptedUrls.Split(',');
            //string vnpReturnUrl = null;

            //foreach (string url in urls)
            //{
            //    if (callBackUrl == url)
            //    {
            //        vnpReturnUrl = url;
            //    }
            //}
            //if (string.IsNullOrEmpty(vnpReturnUrl))
            //{
            //    return "Invalid return url";
            //}

            var pay = new VnPayLibrary();
            //var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.TotalPrice * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"Thanh toan hoa don cho {vnp_OrderInfo}");
            pay.AddRequestData("vnp_OrderType", "250000");
            pay.AddRequestData("vnp_ReturnUrl", callBackUrl);
            //pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", orderId);

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public async Task<bool> PaymentExecute(VnpayModel vnpayModel)
        {
            var order = await _orderRepository.GetOrderByIdAsync(int.Parse(vnpayModel.vnp_TxnRef));
            if (order.UpdatedPayment != null)
            {
                if (order.UpdatedPayment.Value)
                {
                    return false;
                }
            }
            var account = await _userService.GetAccountByUsernameAsync(order.UserName);
            if (order == null)
            {
                return false;
            }
            if (!vnpayModel.vnp_ResponseCode.Equals("00") && !vnpayModel.vnp_TransactionStatus.Equals("00"))
            {
                // send noti to user
                if (account != null)
                {
                    Notification notification = new Notification
                    {
                        AccountId = account.Id,
                        CreateDate = DateTime.Now,
                        Type = NotificationType.Order.ToString(),
                        Title = "Đơn hàng",
                        Action = "Thanh toán thất bại",
                        Message = $"Đơn hàng #{order.Id} thanh toán thất bại. Vui lòng thử lại sau!",
                        ModelId = order.Id
                    };
                    await _notificationService.AddNotificationByAccountId(account.Id, notification);
                }
                bool checkDel = await _apartmentPackageRepo.DeleteApartmentPackage(order.Id);
                order.UpdatedPayment = true;
                int orderErrId = await _orderRepository.UpdateOrderAsync(int.Parse(vnpayModel.vnp_TxnRef), order);
                return false;
            }
            DateTime payDate = DateTime.ParseExact(vnpayModel.vnp_PayDate, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            order.PaymentDate = payDate;
            order.TransactionNo = vnpayModel.vnp_TransactionNo;
            order.UpdatedPayment = true;
            int orderId = await _orderRepository.UpdateOrderAsync(int.Parse(vnpayModel.vnp_TxnRef), order);
            // send noti to user
            if (account != null)
            {
                Notification notification = new Notification
                {
                    AccountId = account.Id,
                    CreateDate = DateTime.Now,
                    Type = NotificationType.Order.ToString(),
                    Title = "Đơn hàng",
                    Action = "Thanh toán thành công",
                    Message = $"Đơn hàng #{order.Id} đã thanh toán thành công. Hãy sử dụng dịch vụ ngay nhé!",
                    ModelId = order.Id
                };
                await _notificationService.AddNotificationByAccountId(account.Id, notification);
            }
            if (order.IsExtraOrder == false)
            {
                int apmPackageId = 0;
                if (orderId != 0)
                {
                    var apartmentPackage = order.ApartmentPackages.SingleOrDefault(a => a.OrderId == orderId);
                    if (apartmentPackage != null)
                    {
                        apartmentPackage.PackageStatus = "Active";
                        apmPackageId = await _apartmentPackageRepo.UpdateApartmentPackageAsync(apartmentPackage.Id, apartmentPackage);
                    }
                }
                if (apmPackageId != 0)
                {
                    return true;
                }

            }
            else
            {
                if (orderId != 0)
                {
                    var apartmentPackage = await _apartmentPackageRepo.GetApartmentPackageByIdAsync(order.ApartmentPackageId.Value);
                    var apmSvId = 0;
                    if (apartmentPackage != null)
                    {
                        // update apartment package service
                        var apartmentService = apartmentPackage.ApartmentPackageServices.SingleOrDefault(a => a.ServiceId == order.ServiceId);
                        //apartmentService.IsExtra = true;
                        apartmentService.CountExtra += 1;
                        apmSvId = await _apartPackageServiceRepo.UpdateApartPackageServiceAsync(apartmentService.Id, apartmentService);
                    }
                    if (apmSvId != 0)
                    {
                        return true;
                    }
                }

            }
            return false;
        }
    }
}
