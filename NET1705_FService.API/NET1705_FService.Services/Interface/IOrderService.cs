using NET1705_FService.Repositories.Data;
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
        public Task<ResponseModel> AddOrderAsync(OrderModel order);
        public Task<ResponseModel> AddExtraOrderAsync(OrderModel extraModel);

    }
}
