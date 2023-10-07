using NET1705_FService.Repositories.Interface;
using NET1705_FService.Repositories.Models;
using NET1705_FService.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Services
{
    public class ApartmentPackageService : IApartmentPackageService
    {
        private readonly IApartmentPackageRepository _repo;

        public ApartmentPackageService(IApartmentPackageRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<ApartmentPackage>> GetAllApartmentPackagesAsync()
        {
            var apartmentPackages = await _repo.GetAllApartmentPackagesAsync();
            return apartmentPackages;
        }

        public async Task<ApartmentPackage> GetApartmentPackageByIdAsync(int id)
        {
            var apartmentPackage = await _repo.GetApartmentPackageByIdAsync(id);
            return apartmentPackage;
        }
    }
}
