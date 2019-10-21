using System.Threading.Tasks;
using AutoMapper;
using Jorros.Vinland.Api.Models;
using Jorros.Vinland.Pricing;
using Microsoft.AspNetCore.Mvc;

namespace Jorros.Vinland.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PricingController : ControllerBase
    {
        private readonly IPricingService _pricingService;

        public PricingController(IPricingService pricingService)
        {
            _pricingService = pricingService;
        }

        [HttpGet("")]
        public async Task<ActionResult<PriceModel>> GetPrice(int amount)
        {
            var shipping = await _pricingService.GetShippingCostsAsync(amount);
            var wine = await _pricingService.GetWineCostsAsync(amount);

            return new PriceModel
            {
                Shipping = shipping,
                Wine = wine
            };
        }
    }
}