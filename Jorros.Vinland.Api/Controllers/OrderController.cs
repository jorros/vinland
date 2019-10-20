using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Jorros.Vinland.Api.Models;
using Jorros.Vinland.OrderProcessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jorros.Vinland.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderModel>> Create(CreateOrderModel createModel)
        {
            var result = await _orderService.CreateOrderAsync(_mapper.Map<CreateOrderRequest>(createModel));

            return CreatedAtAction(nameof(GetById), new { id = result.ReferenceId }, createModel);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetById(Guid id)
        {
            var result = await _orderService.GetOrderAsync(id);

            if(result == null)
            {
                return NotFound();
            }

            return _mapper.Map<OrderModel>(result);
        }

        [HttpGet("name/{name}")]
        public async Task<ActionResult<IEnumerable<OrderModel>>> GetByName(string name)
        {
            var result = await _orderService.GetOrdersAsync(name);

            return _mapper.Map<List<OrderModel>>(result);
        }
    }
}