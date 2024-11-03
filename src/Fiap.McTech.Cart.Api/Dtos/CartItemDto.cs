using System.ComponentModel.DataAnnotations;

namespace Fiap.McTech.Cart.Api.Dtos
{
    public class CartItemDto
    {
        public CartItemDto(string name, int quantity, decimal value, Guid productId, Guid cartClientId)
        {
            Name = name;
            Quantity = quantity;
            Value = value;
            ProductId = productId;
            CartClientId = cartClientId;
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid CartClientId { get; private set; }
    }
}
