using Fiap.McTech.Cart.Api.Controllers;
using Fiap.McTech.Cart.Api.DbContext;
using Fiap.McTech.Cart.Api.Dtos;
using Fiap.McTech.Cart.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Net;

namespace FunctionalTests.StepDefinitions
{
    [Binding]
    public class CartStepDefinitions
    {
        private readonly CartController _controller;
        private IActionResult _response;
        private Guid _clientId;
        private CartItemDto _itemDto;

        public CartStepDefinitions()
        {
            var redisDataContextMock = new Mock<IRedisDataContext>();
            redisDataContextMock.SetupGet(m => m.Database).Returns(Mock.Of<StackExchange.Redis.IDatabase>());
            _controller = new CartController(redisDataContextMock.Object);
        }

        [Given(@"que um carrinho com itens existe para um cliente")]
        public async Task GivenQueUmCarrinhoComItensExisteParaUmCliente()
        {
            _clientId = Guid.NewGuid(); 
            await _controller.CreateCart(_clientId);
            _itemDto = new CartItemDto("Produto Exemplo", 2, 10.0M, Guid.NewGuid(), Guid.NewGuid());

            await _controller.AddItem(_clientId, _itemDto);

            var cartResponse = await _controller.GetCart(_clientId) as OkObjectResult;
            var cart = cartResponse?.Value as CartClient;

            Assert.IsNotNull(cart);
            Assert.IsTrue(cart.Items.Any(i => i.ProductId == _itemDto.ProductId));
        }

        [Given(@"que eu tenho um ID de cliente único")]
        public void GivenQueEuTenhoUmIDDeClienteUnico()
        {
            _clientId = Guid.NewGuid();
        }

        [When(@"eu crio um novo carrinho para o cliente")]
        public async Task WhenEuCrioUmNovoCarrinhoParaOCliente()
        {
            _response = await _controller.CreateCart(_clientId);
        }

        [Then(@"o status da resposta deve ser (.*) Created")]
        public void ThenOStatusDaRespostaDeveSerCreated(int statusCode)
        {
            Assert.AreEqual((HttpStatusCode) statusCode, ((ObjectResult) _response).StatusCode);
        }

        [Then(@"o carrinho deve existir no sistema")]
        public async Task ThenOCarrinhoDeveExistirNoSistema()
        {
            var result = await _controller.GetCart(_clientId) as OkObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual((int) HttpStatusCode.OK, result.StatusCode);
        }

        [Given(@"que um carrinho existe para um cliente")]
        public async Task GivenQueUmCarrinhoExisteParaUmCliente()
        {
            await _controller.CreateCart(_clientId);
        }

        [When(@"eu recupero o carrinho para o cliente")]
        public async Task WhenEuRecuperoOCarrinhoParaOCliente()
        {
            _response = await _controller.GetCart(_clientId);
        }

        [Then(@"o status da resposta deve ser (.*) OK")]
        public void ThenOStatusDaRespostaDeveSerOK(int statusCode)
        {
            Assert.AreEqual((HttpStatusCode) statusCode, ((ObjectResult) _response).StatusCode);
        }

        [Then(@"os Givens do carrinho devem corresponder ao formato esperado")]
        public void ThenOsDadosDoCarrinhoDevemCorresponderAoFormatoEsperado()
        {
            var cart = ((OkObjectResult) _response).Value as CartClient;
            Assert.IsNotNull(cart);
            Assert.AreEqual(_clientId, cart?.ClientId);
        }

        [When(@"eu adiciono um item ao carrinho com detalhes específicos")]
        public async Task WhenEuAdicionoUmItemAoCarrinhoComDetalhesEspecificos()
        {
            _itemDto = new CartItemDto("Produto Exemplo", 2, 10.0M, Guid.NewGuid(), Guid.NewGuid());
            _response = await _controller.AddItem(_clientId, _itemDto);
        }

        [Then(@"o item deve aparecer no carrinho com os detalhes corretos")]
        public async Task ThenOItemDeveAparecerNoCarrinhoComOsDetalhesCorretos()
        {
            var cartResponse = await _controller.GetCart(_clientId) as OkObjectResult;
            var cart = cartResponse!.Value as CartClient;
            var item = cart?.Items.Find(i => i.ProductId == _itemDto.ProductId);

            Assert.IsNotNull(item);
            Assert.AreEqual(_itemDto.Name, item?.Name);
            Assert.AreEqual(_itemDto.Quantity, item?.Quantity);
            Assert.AreEqual(_itemDto.Value, item?.Value);
        }

        [When(@"eu atualizo a quantidade de um item no carrinho")]
        public async Task WhenEuAtualizoAQuantidadeDeUmItemNoCarrinho()
        {
            _response = await _controller.UpdateItemQuantity(_clientId, _itemDto.ProductId, 5);
        }

        [Then(@"a quantidade do item deve ser atualizada no carrinho")]
        public async Task ThenAQuantidadeDoItemDeveSerAtualizadaNoCarrinho()
        {
            var cartResponse = await _controller.GetCart(_clientId) as OkObjectResult;
            var cart = cartResponse?.Value as CartClient;
            var item = cart?.Items.Find(i => i.ProductId == _itemDto.ProductId);

            Assert.IsNotNull(item);
            Assert.AreEqual(5, item.Quantity);
        }

        [When(@"eu removo um item do carrinho")]
        public async Task WhenEuRemovoUmItemDoCarrinho()
        {
            _response = await _controller.RemoveItem(_clientId, _itemDto.ProductId);
        }

        [Then(@"o item não deve mais existir no carrinho")]
        public async Task ThenOItemNaoDeveMaisExistirNoCarrinho()
        {
            var cartResponse = await _controller.GetCart(_clientId) as OkObjectResult;
            var cart = cartResponse!.Value as CartClient;
            var item = cart?.Items.Find(i => i.ProductId == _itemDto.ProductId);

            Assert.IsNull(item);
        }

        [When(@"eu obtenho o valor total do carrinho")]
        public async Task WhenEuObtenhoOValorTotalDoCarrinho()
        {
            _response = await _controller.GetTotalValue(_clientId);
        }

        [Then(@"o valor total deve estar correto")]
        public void ThenOValorTotalDeveEstarCorreto()
        {
            var totalResponse = _response as OkObjectResult;
            Assert.IsNotNull(totalResponse);
            Assert.AreEqual(20.0M, totalResponse?.Value);
        }
    }
}
