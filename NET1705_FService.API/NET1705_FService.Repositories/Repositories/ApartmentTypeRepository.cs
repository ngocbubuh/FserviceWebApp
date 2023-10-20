using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
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
        private readonly IMapper _mapper;

        public ApartmentTypeRepository(FserviceApiDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ApartmentTypeModel>> GetApartmentTypesAsync(int? buildingId)
        {
            var types = _context.ApartmentTypes
                .Include(c => c.Building)
                .AsQueryable();
            if (buildingId != null)
            {
                types = _context.ApartmentTypes
                    .Include(c => c.Building)
                    .Where(t => t.BuildingId == buildingId);
            }
            return types.ProjectTo<ApartmentTypeModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
