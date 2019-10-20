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
            CreateMap<OrderServiceModel, OrderModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.ReferenceId));
            CreateMap<BottleServiceModel, BottleModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.ReferenceId));;
        }
    }
}