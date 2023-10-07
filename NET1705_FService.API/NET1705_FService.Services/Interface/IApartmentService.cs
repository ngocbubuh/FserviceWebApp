using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Inteface
{
    public interface IApartmentService
    {
        public Task<List<Apartment>> GetApartmentOnFloorAsync(int floorId, int typeId);

        public Task<Apartment> GetApartmentByIdAsync(int id);

        public Task<ResponseModel> RegisApartment(int id, string userName);
    }
}
