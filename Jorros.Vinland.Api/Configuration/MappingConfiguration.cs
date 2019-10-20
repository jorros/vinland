using AutoMapper;
using Jorros.Vinland.Api.Models;
using Jorros.Vinland.OrderProcessing;

namespace Jorros.Vinland.Api.Configuration
{
    public class MappingConfiguration : Profile
    {
        public MappingConfiguration()
        {
            CreateMap<CreateOrderModel, CreateOrderRequest>();
            CreateMap<OrderServiceModel, OrderModel>();
        }
    }
}