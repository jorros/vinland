using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jorros.Vinland.OrderProcessing
{
    public interface IOrderService
    {
        Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request);

        Task<OrderServiceModel> GetOrderAsync(Guid id);

        Task<IEnumerable<OrderServiceModel>> GetOrdersAsync(string user);
    }
}