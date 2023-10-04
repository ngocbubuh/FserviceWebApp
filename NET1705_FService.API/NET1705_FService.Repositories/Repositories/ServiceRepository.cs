using Microsoft.EntityFrameworkCore;
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
                _context.Services.Remove(deleteService);
                await _context.SaveChangesAsync();
                return deleteService.Id;
            }
            return 0;
        }

        public async Task<List<Service>> GetAllServiceAsync()
        {
            var services = await _context.Services!.ToListAsync();
            return services;
        }

        public async Task<Service> GetServiceAsync(int id)
        {
            var service = await _context.Services!.FirstOrDefaultAsync(y => y.Id == id);
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
