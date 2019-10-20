using AutoMapper;
using Jorros.Vinland.Data.Entities;

namespace Jorros.Vinland.OrderProcessing.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderServiceModel>();
            CreateMap<Bottle, BottleServiceModel>();
        }
    }
}