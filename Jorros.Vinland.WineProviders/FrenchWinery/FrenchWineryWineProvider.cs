using System;
using System.Threading.Tasks;

namespace Jorros.Vinland.WineProviders.FrenchWinery
{
    public class FrenchWineryWineProvider : IWineProvider
    {
        public Task<OrderBoxResponse> OrderBoxAsync()
        {
            return Task.FromResult(new OrderBoxResponse
            {
                Confirmed = true,
                Reference = Guid.NewGuid().ToString()
            });
        }
    }
}