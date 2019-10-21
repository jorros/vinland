using System.Threading.Tasks;

namespace Jorros.Vinland.Pricing
{
    public interface IPricingService
    {
        Task<float> GetWineCostsAsync(int amountBottles);
        
        Task<float> GetShippingCostsAsync(int amountBottles);
    }
}
