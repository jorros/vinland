using System.Threading.Tasks;
using Jorros.Vinland.Pricing.FrenchWinery;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Jorros.Vinland.Tests.Pricing
{
    public class TFrenchWineryPricingService
    {
        private FrenchWineryPricingSettings _settings = new FrenchWineryPricingSettings
        {
            BottlesInBox = 12,
            Discount = 0.1f,
            PricePerBottle = 1,
            ShippingCostsPerBox = 1
        };

        public FrenchWineryPricingService GetSut()
        {
            return new FrenchWineryPricingService(Options.Create(_settings));
        }
        
        [Test]
        public async Task ABoxOfWineShouldReturnCorrectPrice()
        {
            // Arrange
            var sut = GetSut();

            // Act
            var result = await sut.GetWineCostsAsync(_settings.BottlesInBox);

            // Assert
            Assert.AreEqual(_settings.BottlesInBox * _settings.PricePerBottle, result);
        }

        [Test]
        public async Task MultipleBoxesShouldReturnCorrectShipping()
        {
            // Arrange
            var sut = GetSut();
            const int amountBoxes = 2;

            // Act
            var result = await sut.GetShippingCostsAsync(_settings.BottlesInBox * amountBoxes);

            // Assert
            Assert.AreEqual(amountBoxes * _settings.ShippingCostsPerBox, result);
        }

        [Test]
        public async Task ABoxAndAHalfShouldReturnShippingCostForTwo()
        {
            // Arrange
            var sut = GetSut();
            const int amountBoxes = 2;

            // Act
            var result = await sut.GetShippingCostsAsync(_settings.BottlesInBox + _settings.BottlesInBox / 2);

            // Assert
            Assert.AreEqual(amountBoxes * _settings.ShippingCostsPerBox, result);
        }

        [Test]
        public async Task OrderingOverRequiredAmountShouldApplyDiscount()
        {
            // Arrange
            var sut = GetSut();
            const int amountBoxes = 11;

            // Act
            var result = await sut.GetShippingCostsAsync(_settings.BottlesInBox * amountBoxes);

            // Assert
            Assert.Less(result, amountBoxes * _settings.ShippingCostsPerBox);
        }
    }
}