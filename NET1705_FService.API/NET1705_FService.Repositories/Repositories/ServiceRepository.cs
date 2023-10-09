using Microsoft.EntityFrameworkCore;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;

namespace FServiceAPI.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly FserviceApiDatabaseContext _context;
        public ServiceRepository(FserviceApiDatabaseContext context) 
        {
            _context = context;
        }
        public async Task<int> AddServiceAsync(Service service)
        {
            if (_context.Services == null)
            {
                return 0;
            }
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service.Id;
        }

        public async Task<int> DeleteServiceAsync(int id)
        {
            var deleteService = _context.Services!.SingleOrDefault(x => x.Id == id);
            if (deleteService != null)
            {
                //Delete mềm, ko xóa khỏi database
                //_context.Services.Update(new Service { 
                //    Id = deleteService.Id,
                //    Name = deleteService.Name,
                //    Description = deleteService.Description,
                //    Image = deleteService.Image,
                //    Status = false
                //});
                deleteService.Status = false;
                _context.Services.Update(deleteService);
                await _context.SaveChangesAsync();
                return deleteService.Id;
            }
            return 0;
        }

        public async Task<PagedList<Service>> GetAllServiceAsync(PaginationParameter paginationParameter)
        {
            //var services = await _context.Services!.Where(y => y.Status == true)
            //    .OrderBy(y => y.Id)
            //    .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
            //    .Take(paginationParameter.PageSize)
            //    .ToListAsync();
            //return services;

            var services = await _context.Services!.Where(y => y.Status == true)
                .OrderBy(y => y.Id).ToListAsync();

            return PagedList<Service>.ToPagedList(services,
                paginationParameter.PageNumber,
                paginationParameter.PageSize);
        }

        public async Task<Service> GetServiceAsync(int id)
        {
            var service = await _context.Services!.FirstOrDefaultAsync(y => y.Id == id && y.Status == true);
            return service;
        }

        public async Task<int> UpdateServiceAsync(int id, Service service)
        {
            if (id == service.Id)
            {
                _context.Services!.Update(service);
                await _context.SaveChangesAsync();
                return service.Id;
            }
            return 0;
        }
    }
}
