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
    public class BuildingRepository : IBuildingRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public BuildingRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }
        public async Task<List<Building>> GetAllBuildingAsync()
        {
            var buildings = await _context.Buildings.ToListAsync();
            return buildings;
        }
    }
}
