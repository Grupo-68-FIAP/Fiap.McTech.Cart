using Fiap.McTech.Cart.Api.Controllers;
using Fiap.McTech.Cart.Api.DbContext;
using Fiap.McTech.Cart.Api.Dtos;
using Fiap.McTech.Cart.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StackExchange.Redis;
using System.Text.Json;
using Xunit;

namespace UnitTests.Controllers
{
    public class CartControllerTests
    {
        private readonly Mock<IRedisDataContext> _mockRedisDataContext;
        private readonly Mock<IDatabase> _mockRedisDb;
        private readonly CartController _controller;

        public CartControllerTests()
        {
            _mockRedisDb = new Mock<IDatabase>();
            _mockRedisDataContext = new Mock<IRedisDataContext>();
            _mockRedisDataContext.SetupGet(ctx => ctx.Database).Returns(_mockRedisDb.Object);
            _controller = new CartController(_mockRedisDataContext.Object);
        }

        [Fact]
        public async Task GetCart_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            var clientId = Guid.NewGuid();
            _mockRedisDb.Setup(db => db.StringGetAsync(clientId.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) RedisValue.Null);

            var result = await _controller.GetCart(clientId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetCart_ShouldReturnOk_WhenCartExists()
        {
            var clientId = Guid.NewGuid();
            var cart = new CartClient(clientId);
            var cartData = JsonSerializer.Serialize(cart);

            _mockRedisDb.Setup(db => db.StringGetAsync(clientId.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) cartData);

            var result = await _controller.GetCart(clientId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<CartClient>(okResult.Value);
        }

        [Fact]
        public async Task CreateCart_ShouldReturnBadRequest_WhenCartAlreadyExists()
        {
            var clientId = Guid.NewGuid();
            var existingCart = new CartClient(clientId);
            var cartData = JsonSerializer.Serialize(existingCart);

            _mockRedisDb.Setup(db => db.StringGetAsync(clientId.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) cartData);

            var result = await _controller.CreateCart(clientId);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateCart_ShouldReturnCreated_WhenCartDoesNotExist()
        {
            var clientId = Guid.NewGuid();
            _mockRedisDb
                .Setup(db => db.StringGetAsync(clientId.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) RedisValue.Null);

            _mockRedisDb
                .Setup(db => db.StringSetAsync(clientId.ToString(), It.IsAny<RedisValue>(), null, When.Always, CommandFlags.None))
                .ReturnsAsync(true);

            var result = await _controller.CreateCart(clientId);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.IsType<CartClient>(createdAtActionResult.Value);
        }

        [Fact]
        public async Task AddItem_ShouldReturnNotFound_WhenCartDoesNotExist()
        {
            var clientId = Guid.NewGuid();
            var itemDto = new CartItemDto("Item 1", 1, 10m, Guid.NewGuid(), clientId);

            _mockRedisDb
                .Setup(db => db.StringGetAsync(clientId.ToString(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue) RedisValue.Null);

            var result = await _controller.AddItem(clientId, itemDto);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
