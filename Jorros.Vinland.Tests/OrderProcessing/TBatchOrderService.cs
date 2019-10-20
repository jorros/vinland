using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Jorros.Vinland.Data.Entities;
using Jorros.Vinland.Data.Repositories;
using Jorros.Vinland.OrderProcessing;
using Jorros.Vinland.OrderProcessing.Batch;
using Jorros.Vinland.OrderProcessing.MappingProfiles;
using Jorros.Vinland.WineProviders;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Jorros.Vinland.Tests.OrderProcessing
{
    public class TBatchOrderService
    {
        private BatchOrderSettings _settings = new BatchOrderSettings
        {
            BottlesPerBox = 12
        };
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile(new OrderProfile())).CreateMapper();
        }

        [TestCase(12)]
        [TestCase(14)]
        [TestCase(6)]
        [TestCase(25)]
        public async Task CreateOrderShouldCreateCaseWithBottles(int bottles)
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            var orderRepositoryMock = fixture.Freeze<Mock<IOrderRepository>>();

            fixture.Inject(Options.Create(_settings));

            var sut = fixture.Create<BatchOrderService>();

            // Act
            var result = await sut.CreateOrderAsync(new CreateOrderRequest
            {
                BottlesAmount = bottles,
                User = fixture.Create<string>()
            });

            // Assert
        }

        [TestCase(12)]
        [TestCase(24)]
        public async Task CreateOrderWithFullBatchesShouldCreateConfirmedBottles(int bottles)
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            var orderRepositoryMock = fixture.Freeze<Mock<IOrderRepository>>();
            var wineProviderMock = fixture.Freeze<Mock<IWineProvider>>();

            fixture.Inject(Options.Create(_settings));
            wineProviderMock.Setup(x => x.OrderBoxAsync()).ReturnsAsync(new OrderBoxResponse
            {
                Confirmed = true
            });

            var sut = fixture.Create<BatchOrderService>();

            // Act
            var result = await sut.CreateOrderAsync(new CreateOrderRequest
            {
                BottlesAmount = bottles,
                User = fixture.Create<string>()
            });

            // Assert
            orderRepositoryMock.Verify(x => x.AddAsync(It.Is<Order>(y => y.Bottles.All(x => x.Confirmed))));
        }

        [TestCase(14)]
        [TestCase(26)]
        public async Task CreateOrderWithMoreThanFullBatchesShouldCreateConfirmedAndUnconfirmedBottles(int bottles)
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            var orderRepositoryMock = fixture.Freeze<Mock<IOrderRepository>>();
            var wineProviderMock = fixture.Freeze<Mock<IWineProvider>>();

            fixture.Inject(Options.Create(_settings));
            wineProviderMock.Setup(x => x.OrderBoxAsync()).ReturnsAsync(new OrderBoxResponse
            {
                Confirmed = true
            });

            var sut = fixture.Create<BatchOrderService>();

            var expectedUnconfirmed = bottles % _settings.BottlesPerBox;
            var expectedConfirmed = bottles - expectedUnconfirmed;

            // Act
            var result = await sut.CreateOrderAsync(new CreateOrderRequest
            {
                BottlesAmount = bottles,
                User = fixture.Create<string>()
            });

            // Assert
            orderRepositoryMock.Verify(x => x.AddAsync(
                It.Is<Order>(order =>
                    order.Bottles.Where(z => z.Confirmed).Count() == expectedConfirmed &&
                    order.Bottles.Where(z => !z.Confirmed).Count() == expectedUnconfirmed
                )));
        }

        [TestCase(10, 2)]
        [TestCase(10, 11)]
        public async Task CreateOrderWithPreOrderedBottlesShouldConfirmBottles(int preOrdered, int newOrder)
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization());

            var orderRepositoryMock = fixture.Freeze<Mock<IOrderRepository>>();
            var bottleRepositoryMock = fixture.Freeze<Mock<IBottleRepository>>();
            var wineProviderMock = fixture.Freeze<Mock<IWineProvider>>();

            bottleRepositoryMock.Setup(x => x.GetUnconfirmedAsync()).
                ReturnsAsync(fixture.Build<Bottle>().Without(x => x.Order).CreateMany<Bottle>(preOrdered));

            fixture.Inject(Options.Create(_settings));
            wineProviderMock.Setup(x => x.OrderBoxAsync()).ReturnsAsync(new OrderBoxResponse
            {
                Confirmed = true
            });

            var sut = fixture.Create<BatchOrderService>();

            var totalBottles = preOrdered + newOrder;
            var expectedUnconfirmed = totalBottles % _settings.BottlesPerBox;
            var expectedConfirmed = totalBottles - expectedUnconfirmed - preOrdered;

            // Act
            var result = await sut.CreateOrderAsync(new CreateOrderRequest
            {
                BottlesAmount = newOrder,
                User = fixture.Create<string>()
            });

            // Assert
            bottleRepositoryMock.Verify(x => x.UpdateRangeAsync(
                It.Is<IEnumerable<Bottle>>(bottles =>
                    bottles.All(x => x.Confirmed)
                )));
            orderRepositoryMock.Verify(x => x.AddAsync(
                It.Is<Order>(order =>
                    order.Bottles.Where(z => z.Confirmed).Count() == expectedConfirmed &&
                    order.Bottles.Where(z => !z.Confirmed).Count() == expectedUnconfirmed
                )));
        }

        [Test]
        public async Task GetOrderShouldReturnOrder()
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true });

            var id = fixture.Create<Guid>();

            fixture.Inject(_mapper);
            var expectedOrder = fixture.Build<Order>().Without(x => x.Bottles).Create();
            fixture.Inject(expectedOrder);

            var sut = fixture.Create<BatchOrderService>();

            // Act
            var order = await sut.GetOrderAsync(fixture.Create<Guid>());

            // Assert
            Assert.IsNotNull(order);
            Assert.AreEqual(expectedOrder.ReferenceId, order.ReferenceId);
        }

        [Test]
        public async Task GetOrdersShouldReturnOrders()
        {
            // Arrange
            var fixture = new Fixture()
                .Customize(new AutoMoqCustomization { ConfigureMembers = true });

            var id = fixture.Create<Guid>();

            fixture.Inject(_mapper);
            fixture.Inject(fixture.Build<Order>().Without(x => x.Bottles).Create());

            var sut = fixture.Create<BatchOrderService>();

            // Act
            var orders = await sut.GetOrdersAsync(fixture.Create<string>());

            // Assert
            Assert.IsNotEmpty(orders);
        }
    }
}