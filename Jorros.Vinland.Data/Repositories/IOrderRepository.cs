using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jorros.Vinland.Data.Entities;

namespace Jorros.Vinland.Data.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);

        Task<Order> GetByReferenceAsync(Guid referenceId);

        Task<IEnumerable<Order>> GetByUserAsync(string user);
    }
}