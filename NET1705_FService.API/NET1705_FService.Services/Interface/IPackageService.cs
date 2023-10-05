using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IPackageService
    {
        public Task<List<Package>> GetAllPackagesAsync();
        public Task<Package> GetPackageAsync(int id);
        public Task<ResponseModel> AddPackageAsync(Package package);
        public Task<ResponseModel> UpdatePackageAsync(int id, Package package);
        public Task<ResponseModel> DeletePackageAsync(int id);
    }
}
