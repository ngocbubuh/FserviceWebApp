using NET1705_FService.Repositories.Helper;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Interface
{
    public interface IOrderRepository
    {
        public Task<int> AddOrderAsync(OrderModel orderModel);

        public Task<int> AddExtraOrderAsync(OrderModel extraModel);

        public Task<PagedList<Order>> GetOrderByUserNameAsync(PaginationParameter paginationParameter, string userName);

        public Task<PagedList<Order>> GetAllOrdersAsync(PaginationParameter paginationParameter);

        public Task<int> UpdateOrderAsync(int id, Order order);

        public Task<Order> GetOrderByIdAsync(int id);
    }
}
