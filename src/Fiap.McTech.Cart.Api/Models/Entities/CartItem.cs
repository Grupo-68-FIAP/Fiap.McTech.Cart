namespace Fiap.McTech.Cart.Api.Models.Entities
{
    public class CartItem
    {
        public CartItem(string name, int quantity, decimal value, Guid productId, Guid cartClientId)
        {
            Name = name;
            Quantity = quantity;
            Value = value;
            ProductId = productId;
            CartClientId = cartClientId;

            if (!IsValid())
            {
                throw new InvalidOperationException("Invalid item details provided.");
            }
        }

        public string Name { get; private set; }
        public int Quantity { get; private set; }
        public decimal Value { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid CartClientId { get; private set; }

        public void AddUnities(int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidOperationException("Quantity must be greater than zero.");
            }
            Quantity += quantity;
        }

        public void UpdateUnities(int quantity)
        {
            if (quantity < 0)
            {
                throw new InvalidOperationException("Quantity cannot be negative.");
            }

            Quantity = quantity;
        }

        public decimal CalculateValue()
        {
            return Quantity * Value;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   Quantity > 0 &&
                   Value > 0 &&
                   ProductId != Guid.Empty &&
                   CartClientId != Guid.Empty;
        }
    }
}
