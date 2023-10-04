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
    public class FloorRepository : IFloorRepository
    {
        private readonly FserviceApiDatabaseContext _context;

        public FloorRepository(FserviceApiDatabaseContext context)
        {
            _context = context;
        }
        public Task<List<Floor>> GetAllFloorsOfBuilding(int buidingId)
        {
            var floors = _context.Floors.Where(f => f.BuildingId == buidingId).ToListAsync();
            return floors;
        }
    }
}
