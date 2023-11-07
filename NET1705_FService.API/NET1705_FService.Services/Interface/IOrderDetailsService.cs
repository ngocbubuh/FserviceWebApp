using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Services.Interface
{
    public interface IOrderDetailsService
    {
        public Task<OrderDetailsViewModel> GetOrderDetailsByIdAsync(int id);
        public Task<ResponseModel> AddOrderDetails(UsingPackageModel usingPackage);

        public Task<PagedList<OrderDetailsViewModel>> GetAllTaskForStaffAsync(PaginationParameter paginationParameter, string username);

        public Task<int> UpdateTaskAsync(int id, OrderDetailModel orderDetailModel);

        public Task<PagedList<OrderDetailsViewModel>> GetOrderDetailByApartmentPackageId(PaginationParameter paginationParameter, int apartmentPackageId);

    }
}
