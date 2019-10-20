using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jorros.Vinland.Data.Entities;

namespace Jorros.Vinland.Data.Repositories
{
    public class BottleRepository : IBottleRepository
    {
        private readonly VinlandContext _context;

        public BottleRepository(VinlandContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Bottle bottle)
        {
            await _context.Bottles.AddAsync(bottle);

            await _context.SaveChangesAsync();
        }

        public Task<Bottle> GetByReferenceAsync(Guid referenceId)
        {
            return Task.FromResult(_context.Bottles.FirstOrDefault(x => x.ReferenceId == referenceId));
        }

        public Task<IEnumerable<Bottle>> GetUnconfirmedAsync()
        {
            return Task.FromResult((IEnumerable<Bottle>)_context.Bottles.Where(x => !x.Confirmed).ToList());
        }

        public async Task UpdateRangeAsync(IEnumerable<Bottle> bottles)
        {
            _context.Bottles.UpdateRange(bottles);

            await _context.SaveChangesAsync();
        }
    }
}