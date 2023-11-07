using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IApartmentPackageRepository
    {
        public Task<int> AddApartmentPackageAsync(ApartmentPackage apartmentPackage);

        public Task<ApartmentPackage> GetApartmentPackageByIdAsync(int id);

        public Task<PagedList<ApartmentPackage>> GetAllApartmentPackagesAsync(PaginationParameter paginationParameter);

        public Task<int> UpdateApartmentPackageAsync(int id, ApartmentPackage apartmentPackage);

        public Task<PagedList<ApartmentPackage>> GetApartmentPackagesByApartmentId(PaginationParameter paginationParameter, int apartmentId);

        public Task<bool> CheckApartmentPackagesByApartmentAndPackage(int apartmentId, int packageId);

        public Task<bool> DeleteApartmentPackage(int orderId);
    }
}
