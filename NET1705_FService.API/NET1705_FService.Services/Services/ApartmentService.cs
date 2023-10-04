using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _repo;

        public ApartmentService(IApartmentRepository repo)
        {
            _repo = repo;
        }
        public async Task<Apartment> GetApartmentByIdAsync(int id)
        {
            var apartment = await _repo.GetApartmentByIdAsync(id);
            return apartment;
        }

        public async Task<List<Apartment>> GetApartmentOnFloorAsync(int floorId, int typeId)
        {
            var apartments = await _repo.GetApartmentsOnFloorAsync(floorId, typeId);
            return apartments;
        }
    }
}
