using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jorros.Vinland.Data.Entities;

namespace Jorros.Vinland.Data.Repositories
{
    public interface IBottleRepository
    {
        Task AddAsync(Bottle bottle);

        Task<Bottle> GetByReferenceAsync(Guid referenceId);

        Task<IEnumerable<Bottle>> GetUnconfirmedAsync();

        Task UpdateRangeAsync(IEnumerable<Bottle> bottles);
    }
}