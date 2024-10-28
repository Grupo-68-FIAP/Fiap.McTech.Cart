using Fiap.McTech.Cart.Api.DataContext;
using Fiap.McTech.Cart.Api.Models.Dtos;
using Fiap.McTech.Cart.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Text.Json;

namespace Fiap.McTech.Cart.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IDatabase _redisDb;

        public CartController(RedisDataContext redisDataContext)
        {
            _redisDb = redisDataContext.Database;
        }

        [HttpGet("{clientId}")]
        [ProducesResponseType(typeof(CartClient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCart(Guid clientId)
        {
            var cart = await GetCartFromRedis(clientId);
            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CartClient), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCart([FromBody] Guid clientId)
        {
            var existingCart = await GetCartFromRedis(clientId);
            if (existingCart != null)
                return BadRequest("A cart already exists for this client.");

            var cart = new CartClient(clientId);
            await SaveCartToRedis(cart);

            return CreatedAtAction(nameof(GetCart), new { clientId }, cart);
        }

        [HttpPost("{clientId}/items")]
        [ProducesResponseType(typeof(CartClient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddItem(Guid clientId, [FromBody] CartItemDto itemDto)
        {
            var cart = await GetCartFromRedis(clientId);
            if (cart == null)
                return NotFound();

            cart.AddItem(itemDto.Name, itemDto.Quantity, itemDto.Value, itemDto.ProductId);
            await SaveCartToRedis(cart);

            return Ok(cart);
        }

        [HttpPut("{clientId}/items/{productId}")]
        [ProducesResponseType(typeof(CartClient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateItemQuantity(Guid clientId, Guid productId, [FromQuery] int quantity)
        {
            var cart = await GetCartFromRedis(clientId);
            if (cart == null)
                return NotFound();

            cart.UpdateItemQuantity(productId, quantity);
            await SaveCartToRedis(cart);
            return Ok(cart);
        }

        [HttpDelete("{clientId}/items/{productId}")]
        [ProducesResponseType(typeof(CartClient), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveItem(Guid clientId, Guid productId)
        {
            var cart = await GetCartFromRedis(clientId);
            if (cart == null)
                return NotFound();

            cart.RemoveItem(productId);
            await SaveCartToRedis(cart);
            return Ok(cart);
        }

        [HttpGet("{clientId}/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTotalValue(Guid clientId)
        {
            var cart = await GetCartFromRedis(clientId);
            if (cart == null)
                return NotFound();

            var total = cart.CalculateTotalValue();
            return Ok(total);
        }

        private async Task<CartClient?> GetCartFromRedis(Guid clientId)
        {
            var cartData = await _redisDb.StringGetAsync(clientId.ToString());
            return cartData.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CartClient>(cartData);
        }

        private async Task SaveCartToRedis(CartClient cart)
        {
            var cartData = JsonSerializer.Serialize(cart);
            await _redisDb.StringSetAsync(cart.ClientId.ToString(), cartData);
        }
    }
}
