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
    public class ApartmentTypeService : IApartmentTypeService
    {
        private readonly IApartmentTypeRepository _repo;

        public ApartmentTypeService(IApartmentTypeRepository repo)
        {
            _repo = repo;
        }

        public Task<List<ApartmentType>> GetAllApartmentTypesAsync(int buildingId)
        {
            var apartmentTypes = _repo.GetApartmentTypesAsync(buildingId);
            return apartmentTypes;
        }
    }
}
