using System.Text.Json.Serialization;

namespace Fiap.McTech.Cart.Api.Entities
{
    public class CartClient : EntityBase
    {
        public Guid ClientId { get; private set; }
        public List<CartItem> Items { get; private set; } = new();

        [JsonConstructor] 
        public CartClient(Guid clientId, List<CartItem> items)
        {
            ClientId = clientId;
            Items = items ?? new List<CartItem>();
        }

        public CartClient(Guid clientId)
        {
            ClientId = clientId;
        }

        public void AddItem(string name, int quantity, decimal value, Guid productId)
        {
            CartItem? existingItem = Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.AddUnities(quantity);
            }
            else
            {
                var newItem = new CartItem(name, quantity, value, productId, Id);
                if (!newItem.IsValid())
                    throw new InvalidOperationException("Invalid item details provided.");

                Items.Add(newItem);
            }
        }

        public void UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                throw new InvalidOperationException("Item not found in cart.");

            item.UpdateUnities(quantity);
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal CalculateTotalValue()
        {
            return Items.Sum(item => item.CalculateValue());
        }

        public override bool IsValid()
        {
            return ClientId != Guid.Empty && Items.All(item => item.IsValid());
        }
    }
}
