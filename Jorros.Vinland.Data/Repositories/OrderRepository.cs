using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jorros.Vinland.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jorros.Vinland.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly VinlandContext _context;

        public OrderRepository(VinlandContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            var entity = await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();
        }

        public Task<Order> GetByReferenceAsync(Guid referenceId)
        {
            return Task.FromResult(_context.Orders.FirstOrDefault(x => x.ReferenceId == referenceId));
        }

        public Task<IEnumerable<Order>> GetByUserAsync(string user)
        {
            return Task.FromResult((IEnumerable<Order>)_context.Orders.Where(x => x.User == user).Include(x => x.Bottles).ToList());
        }
    }
}