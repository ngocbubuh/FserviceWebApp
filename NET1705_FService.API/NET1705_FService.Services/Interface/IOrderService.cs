using Microsoft.AspNetCore.Http;
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
    public interface IOrderService
    {
        public Task<ResponseModel> AddOrderAsync(OrderModel order, HttpContext httpContext);
        public Task<ResponseModel> AddExtraOrderAsync(OrderModel extraModel, HttpContext httpContext);

        public Task<PagedList<Order>> GetOrdersByUserName(PaginationParameter paginationParameter, string userName);

        public Task<PagedList<Order>> GetAllOrdersAsync(PaginationParameter paginationParameter);

        public Task<ResponseModel> UpdateOrderAsync(int id, Order order);

        public Task<Order> GetOrderByIdAsync(int id);
    }
}
