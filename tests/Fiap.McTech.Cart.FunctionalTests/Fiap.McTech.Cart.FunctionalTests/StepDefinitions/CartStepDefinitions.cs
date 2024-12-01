using Fiap.McTech.Cart.Api.Controllers;
using Fiap.McTech.Cart.Api.DbContext;
using Fiap.McTech.Cart.Api.Dtos;
using Fiap.McTech.Cart.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;
using System.Text.Json;

namespace FunctionalTests.StepDefinitions
{
    [Binding]
    public class CartStepDefinitions
    {
        private readonly Mock<IRedisDataContext> _redisDataContextMock;
        private readonly CartController _controller;
        private IActionResult _response;
        private readonly Guid _clientId;
        private Mock<IDatabase> _redisDbMock;

        public CartStepDefinitions()
        {
            _redisDbMock = new Mock<IDatabase>();
            _redisDataContextMock = new Mock<IRedisDataContext>();
            _redisDataContextMock.Setup(r => r.Database).Returns(_redisDbMock.Object);
            _controller = new CartController(_redisDataContextMock.Object);
            _clientId = Guid.NewGuid();
        }

        [Given(@"que eu tenho um ID de cliente único")]
        public void GivenQueEuTenhoUmIDDeClienteUnico()
        {
            // No actions needed here
        }

        [When(@"eu crio um novo carrinho para o cliente")]
        public async Task WhenEuCrioUmNovoCarrinhoParaOCliente()
        {
            _redisDbMock
                .Setup(r => r.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .ReturnsAsync(RedisValue.Null);

            _response = await _controller.CreateCart(_clientId);
        }

        [Then(@"o status da resposta deve ser (.*) Created")]
        public void ThenOStatusDaRespostaDeveSerCreated(int statusCode)
        {
            var createdResult = _response as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(statusCode, createdResult?.StatusCode);
        }

        [Then(@"o carrinho deve existir no sistema")]
        public async Task ThenOCarrinhoDeveExistirNoSistema()
        {
            _redisDbMock
                .Verify(r => r.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), null, false, When.Always, CommandFlags.None), Times.Once);

            var cartData = JsonSerializer.Serialize(new CartClient(_clientId));
            var redisValue = new RedisValue(cartData);  

            _redisDbMock
                .Setup(r => r.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .ReturnsAsync(redisValue);

            var result = await _controller.GetCart(_clientId);
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);
        }

        [Given(@"que um carrinho existe para um cliente")]
        public void GivenQueUmCarrinhoExisteParaUmCliente()
        {
            var cartData = JsonSerializer.Serialize(new CartClient(_clientId));
            _redisDbMock
                .Setup(r => r.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .ReturnsAsync(cartData); 
        }

        [When(@"eu recupero o carrinho para o cliente")]
        public async Task WhenEuRecuperoOCarrinhoParaOCliente()
        {
            _response = await _controller.GetCart(_clientId);
        }

        [Then(@"o status da resposta deve ser (.*) OK")]
        public void ThenOStatusDaRespostaDeveSerOK(int statusCode)
        {
            var okResult = _response as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(statusCode, okResult?.StatusCode);
        }

        [Then(@"os Dados do carrinho devem corresponder ao formato esperado")]
        public void ThenOsDadosDoCarrinhoDevemCorresponderAoFormatoEsperado()
        {
            var result = _response as OkObjectResult;
            Assert.IsNotNull(result);
            var cart = result?.Value as CartClient;
            Assert.IsNotNull(cart);
            Assert.AreEqual(_clientId, cart?.ClientId);
        }

        [When(@"eu adiciono um item ao carrinho com detalhes específicos")]
        public async Task WhenEuAdicionoUmItemAoCarrinhoComDetalhesEspecificos()
        {
            var itemDto = new CartItemDto(name: "Item Exemplo", quantity: 2, value: 15.99m, productId: Guid.NewGuid(), _clientId);

            var cartData = JsonSerializer.Serialize(new CartClient(_clientId));
            var redisValue = new RedisValue(cartData);

            _redisDbMock
                .Setup(r => r.StringGetAsync(It.IsAny<RedisKey>(), CommandFlags.None))
                .ReturnsAsync(redisValue);

            _response = await _controller.AddItem(_clientId, itemDto);
        }

        [Then(@"o item deve aparecer no carrinho com os detalhes corretos")]
        public void ThenOItemDeveAparecerNoCarrinhoComOsDetalhesCorretos()
        {
            var okResult = _response as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult?.StatusCode);

            var cart = okResult?.Value as CartClient;
            Assert.IsNotNull(cart);

            var item = cart?.Items.SingleOrDefault(i => i.ProductId == cart?.Items.First().ProductId);
            Assert.IsNotNull(item);
            Assert.AreEqual("Item Exemplo", item?.Name);
            Assert.AreEqual(2, item?.Quantity);
            Assert.AreEqual(15.99m, item?.Value);
        }
    }
}
