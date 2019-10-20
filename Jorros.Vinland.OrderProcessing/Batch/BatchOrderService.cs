using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Jorros.Vinland.Data.Entities;
using Jorros.Vinland.Data.Repositories;
using Jorros.Vinland.WineProviders;
using Microsoft.Extensions.Options;

namespace Jorros.Vinland.OrderProcessing.Batch
{
    public class BatchOrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBottleRepository _bottleRepository;
        private readonly IWineProvider _wineProvider;
        private readonly BatchOrderSettings _settings;
        private readonly IMapper _mapper;

        public BatchOrderService(IMapper mapper, IOrderRepository orderRepository,
        IBottleRepository bottleRepository, IWineProvider wineProvider, IOptions<BatchOrderSettings> settings)
        {
            _orderRepository = orderRepository;
            _bottleRepository = bottleRepository;
            _wineProvider = wineProvider;
            _settings = settings.Value;
            _mapper = mapper;
        }

        public async Task<CreateOrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var bottles = new List<Bottle>();
            var numFullBatchesInOrder = Math.Floor((double)request.BottlesAmount / _settings.BottlesPerBox);

            for (var i = 0; i < numFullBatchesInOrder; i++)
            {
                var orderResult = await _wineProvider.OrderBoxAsync();
                AddBottles(bottles, _settings.BottlesPerBox, orderResult);
            }

            var bottlesExtra = request.BottlesAmount % _settings.BottlesPerBox;

            if (bottlesExtra > 0)
            {
                var previousBatch = (await _bottleRepository.GetUnconfirmedAsync()).ToList();
                var totalBottles = previousBatch.Count + bottlesExtra;

                if (totalBottles >= _settings.BottlesPerBox)
                {
                    var orderResult = await _wineProvider.OrderBoxAsync();
                    previousBatch.ForEach(x =>
                    {
                        x.Confirmed = orderResult.Confirmed;
                        x.WineryOrderReference = orderResult.Reference;
                    });

                    // Add confirmed bottles to fill up batch
                    AddBottles(bottles, _settings.BottlesPerBox - previousBatch.Count, orderResult);

                    // Add rest as unconfirmed
                    AddBottles(bottles, totalBottles - _settings.BottlesPerBox);

                    await _bottleRepository.UpdateRangeAsync(previousBatch);
                }
                else
                {
                    AddBottles(bottles, bottlesExtra);
                }
            }

            var order = new Order
            {
                OrderDate = DateTime.Now,
                ReferenceId = Guid.NewGuid(),
                User = request.User,
            };

            order.Bottles = bottles;

            var id = await _orderRepository.AddAsync(order);

            return new CreateOrderResponse { Id = id };
        }

        private void AddBottles(List<Bottle> bottles, int amount, OrderBoxResponse wineryResponse = null)
        {
            for (var i = 0; i < amount; i++)
            {
                bottles.Add(new Bottle
                {
                    ReferenceId = Guid.NewGuid(),
                    Confirmed = wineryResponse?.Confirmed ?? false,
                    WineryOrderReference = wineryResponse?.Reference
                });
            }
        }

        public async Task<OrderServiceModel> GetOrderAsync(Guid id)
        {
            var result = await _orderRepository.GetByReferenceAsync(id);

            return _mapper.Map<OrderServiceModel>(result);
        }

        public async Task<IEnumerable<OrderServiceModel>> GetOrdersAsync(string user)
        {
            var result = await _orderRepository.GetByUserAsync(user);

            return _mapper.Map<IEnumerable<OrderServiceModel>>(result);
        }
    }
}