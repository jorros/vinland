using System;

namespace Jorros.Vinland.Pricing
{
    public interface IPricingService
    {
        float GetWineCosts(int amountBottles);
        
        float GetShippingCosts(int amountBottles);
    }
}
