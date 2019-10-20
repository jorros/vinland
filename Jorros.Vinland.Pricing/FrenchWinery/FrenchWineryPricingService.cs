using System;
using Microsoft.Extensions.Options;

namespace Jorros.Vinland.Pricing.FrenchWinery
{
    public class FrenchWineryPricingService : IPricingService
    {
        private readonly FrenchWineryPricingSettings _settings;

        public FrenchWineryPricingService(IOptions<FrenchWineryPricingSettings> settings)
        {
            _settings = settings.Value;
        }
        
        public float GetShippingCosts(int amountBottles)
        {
            var numBoxes = (float)Math.Ceiling((double)amountBottles / _settings.BottlesInBox);
            float shipping = numBoxes * _settings.ShippingCostsPerBox;

            if(numBoxes > 10)
            {
                shipping = shipping - shipping * _settings.Discount;
            }

            return shipping;
        }

        public float GetWineCosts(int amountBottles)
        {
            return amountBottles * _settings.PricePerBottle;
        }
    }
}