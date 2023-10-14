using FServiceAPI.Repositories;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using NET1715_FService.Service.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NET1715_FService.Service.Services
{
    public class ServiceService : IServiceService
    {
        private readonly IServiceRepository _repo;

        public ServiceService(IServiceRepository repo)
        {
            _repo = repo;
        }

        public async Task<ResponseModel> AddServiceAsync(NET1705_FService.Repositories.Models.Service service)
        {
            var result = await _repo.AddServiceAsync(service);
            if (result != 0)
            {
                return new ResponseModel { Status = "Success", Message = result.ToString() };
            }
            return new ResponseModel { Status = "Error", Message = "Error! Something went wrong, please try again!" };
        }

        public async Task<ResponseModel> DeleteServiceAsync(int id)
        {
            var deleteService = await _repo.GetServiceAsync(id);
            if (deleteService != null)
            {
                var result = await _repo.DeleteServiceAsync(id);
                if (result != 0)
                {
                    return new ResponseModel { Status = "Success", Message = $"Delete successfully Service {deleteService.Name}!" };
                }
                return new ResponseModel { Status = "Error", Message = $"Cannot delete Service {deleteService.Name}!" };
            }
            return new ResponseModel { Status = "Error", Message = $"Not found Service Id {id}!" };
        }

        public async Task<PagedList<NET1705_FService.Repositories.Models.Service>> GetAllServicesAsync(PaginationParameter paginationParameter)
        {
            var services = await _repo.GetAllServiceAsync(paginationParameter);
            return services;
        }

        public async Task<NET1705_FService.Repositories.Models.Service> GetServiceAsync(int id)
        {
            var service = await _repo.GetServiceAsync(id);
            return service;
        }

        public async Task<ResponseModel> UpdateServiceAsync(int id, NET1705_FService.Repositories.Models.Service service)
        {
            if (id == service.Id)
            {
                var result = await _repo.UpdateServiceAsync(id, service);
                if (result != 0)
                {
                    return new ResponseModel { Status = "Success", Message = result.ToString() };
                }
                return new ResponseModel { Status = "Error", Message = "Update error!" };
            }
            return new ResponseModel { Status = "Error", Message = "Service not found!" };
        }
    }
}
