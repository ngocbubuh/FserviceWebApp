using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Inteface
{
    public interface IApartmentRepository
    {
        public Task<List<ApartmentModel>> GetApartmentsAsync(int? floorId, int? typeId, string? username);
        public Task<Apartment> GetApartmentByIdAsync(int id);

        public Task<int> RegisApartmentAsync(int id, string accountId);

        public Task<List<ApartmentModel>> GetApartmentsByUserNameAsync(string userName);
    }
}
