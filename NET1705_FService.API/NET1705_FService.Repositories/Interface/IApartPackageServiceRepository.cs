using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IApartPackageServiceRepository
    {
        public Task<int> AddApartPackageServiceAsync(ApartmentPackageService apartmentPackageService);

        //public Task<List<ApartmentPackageService>> GetAllApartPackageServiceByStaffIdAsync(int staffId);

        public Task<int> UpdateApartPackageServiceAsync(int id, ApartmentPackageService apartmentPackageService);
    }
}
