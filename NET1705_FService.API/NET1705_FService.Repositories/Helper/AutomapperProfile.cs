using AutoMapper;
using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET1705_FService.Repositories.Helper
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile() 
        { 
            CreateMap<OrderModel, Order>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailsViewModel>().ReverseMap();
            CreateMap<UsingPackageModel, OrderDetail>();
            // NgocBuh
            CreateMap<AccountsModel, Accounts>().ReverseMap();
        }
    }
}
