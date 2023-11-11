using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IOrderDetailsRepository
    {
        public Task<OrderDetailsViewModel> GetOrderDetailByIdAsync(int id);
        public Task<ResponseModel> AddOrderDetailsAsync(UsingPackageModel usingPackage);

        public Task<PagedList<OrderDetailsViewModel>> GetAllTaskForStaff(PaginationParameter paginationParameter, string username);

        public Task<int> UpdateTaskAsync(int id, OrderDetailModel orderDetailModel);

        public Task<PagedList<OrderDetailsViewModel>> GetOrderDetailByApartmentPackageId(PaginationParameter paginationParameter, int apartmentPackageId);

        public Task<int> ConfirmStaffWork(int id, OrderDetailModel orderDetailModel);
    }
}
