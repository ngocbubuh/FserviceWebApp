using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Models;
using NET1715_FService.API.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1715_FService.API.Repository.Repositories
{
    public class ApartmentTypeRepository : IApartmentTypeRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public ApartmentTypeRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<ApartmentType>> GetApartmentTypesAsync(int buildingId)
        {
            var types = await _context.ApartmentTypes.Where(t => t.BuildingId == buildingId).ToListAsync();
            return types;
        }
    }
}
