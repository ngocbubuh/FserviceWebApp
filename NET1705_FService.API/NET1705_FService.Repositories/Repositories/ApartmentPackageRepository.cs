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
    public class ApartmentPackageRepository : IApartmentPackageRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IApartmentRepository _apartmentRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly IPackageRepository _packageRepo;

        public ApartmentPackageRepository(FserviceApiDatabaseContext context, 
            IUserRepository userRepo, IApartmentRepository apartmentRepo, 
            INotificationRepository notificationRepo, IPackageRepository packageRepo)
        {
            _context = context;
            _userRepo = userRepo;
            _apartmentRepo = apartmentRepo;
            _notificationRepo = notificationRepo;
            _packageRepo = packageRepo;
        }
        public async Task<int> AddApartmentPackageAsync(ApartmentPackage apartmentPackage)
        {
            if (_context == null)
            {
                return 0;
            }
            _context.Add(apartmentPackage);
            await _context.SaveChangesAsync();
            return apartmentPackage.Id;
        }

        public async Task<PagedList<ApartmentPackage>> GetAllApartmentPackagesAsync(PaginationParameter paginationParameter)
        {
            //var apartmentPackages = await _context.ApartmentPackages.Where(a => a.PackageStatus.Equals("Active")).ToListAsync();
            var apartmentPackages = await _context.ApartmentPackages.ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active" && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            return PagedList<ApartmentPackage>.ToPagedList(apartmentPackages,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<ApartmentPackage> GetApartmentPackageByIdAsync(int id)
        {
            var apartmentPackage = await _context.ApartmentPackages
                .Include(a => a.ApartmentPackageServices).ThenInclude(ad => ad.Service)
                .Include(a => a.Apartment)
                .Include(a => a.Package)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (apartmentPackage != null)
            {
                if (apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }

            return apartmentPackage;
        }

        // check before buy new package
        public async Task<bool> CheckApartmentPackagesByApartmentAndPackage(int apartmentId, int packageId)
        {
            var apartmentPackages = await _context.ApartmentPackages
                .Where(a => a.ApartmentId == apartmentId)
                .ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active"
                    && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            var activePackages = apartmentPackages.Where(a => a.PackageId == packageId && a.PackageStatus == "Active");
            if (activePackages.Any())
            {
                return false;
            }
            return true;
        }

        public async Task<PagedList<ApartmentPackage>> GetApartmentPackagesByApartmentId(PaginationParameter paginationParameter, int apartmentId)
        {
            var apartmentPackages = await _context.ApartmentPackages
                .Where(a => a.ApartmentId == apartmentId)
                //.Include(a => a.ApartmentPackageServices)
                .Include(a => a.Package)
                .ToListAsync();
            foreach (var apartmentPackage in apartmentPackages)
            {
                if (apartmentPackage.PackageStatus == "Active"
                    && apartmentPackage.EndDate < DateTime.Now.Date)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();
                }
            }
            return PagedList<ApartmentPackage>.ToPagedList(apartmentPackages,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<int> UpdateApartmentPackageAsync(int id, ApartmentPackage apartmentPackage)
        {
            if (id == apartmentPackage.Id)
            {
                _context.Update(apartmentPackage);
                await _context.SaveChangesAsync();
                return apartmentPackage.Id;
            }
            return 0;
        }

        public async Task<bool> DeleteApartmentPackage(int orderId)
        {
            var apartmentPackage = _context.ApartmentPackages
                .Include(a => a.ApartmentPackageServices)
                .FirstOrDefault(a => a.OrderId == orderId && a.PackageStatus == "Disable");
            if (apartmentPackage != null)
            {
                _context.ApartmentPackages.Remove(apartmentPackage);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> CheckExpiredAllApartmentpackage()
        {
            try
            {
                var apartmentPackages = await _context.ApartmentPackages
                    .Where(ap => ap.PackageStatus == "Active" && ap.EndDate < DateTime.Now.Date)
                    .ToListAsync();

                foreach (var apartmentPackage in apartmentPackages)
                {
                    apartmentPackage.PackageStatus = "Expired";
                    _context.Update(apartmentPackage);
                    await _context.SaveChangesAsync();

                    // send notification
                    var ownerAccountId = await GetOwnerApartmentPackage(apartmentPackage);
                    if (!string.IsNullOrEmpty(ownerAccountId))
                    {
                        await SendNotificationToCustomer(apartmentPackage, ownerAccountId);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
                return 0;
            }
        }

        // tools
        private async Task<string> GetOwnerApartmentPackage(ApartmentPackage apartmentPackage)
        {
            var apartment = await _apartmentRepo.GetApartmentByIdAsync(apartmentPackage.ApartmentId.Value);
            return apartment?.AccountId;
        }

        private async Task<int> SendNotificationToCustomer(ApartmentPackage apartmentPackage, string accountId)
        {
            var package = await _packageRepo.GetPackageAsync(apartmentPackage.PackageId, 0);
            if (package != null)
            {
                var notification = new Notification
                {
                    AccountId = accountId,
                    CreateDate = DateTime.Now,
                    Type = NotificationType.Service.ToString(),
                    Title = "Gói đã hết hạn",
                    Action = "Gói dịch vụ",
                    Message = $"{package.Name} đã hết hạn sử dụng. Vui lòng mua gói dịch vụ khác để tiếp tục sử dụng!",
                    ModelId = apartmentPackage.Id
                };

                return await _notificationRepo.AddNotificationByAccountId(accountId, notification);
            }
            return 0;
        }

    }
}
