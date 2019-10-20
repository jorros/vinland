using System.Threading.Tasks;
using AutoMapper;
using Jorros.Vinland.Api.Models;
using Jorros.Vinland.OrderProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jorros.Vinland.Api.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderModel>> Create(OrderModel model)
        {
            var result = await _orderService.CreateOrderAsync(_mapper.Map<CreateOrderRequest>(model));

            model.Id = result.Id;

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, model);
        }

        [HttpGet]
        public async Task<ActionResult<OrderModel>> GetById(int id)
        {
            var result = await _orderService.GetOrderAsync(id);

            return _mapper.Map<OrderModel>(result);
        }
    }
}