using Fiap.McTech.Cart.Api.Entities;
using Xunit;

namespace UnitTests.Entities
{
    public class CartItemTests
    {
        [Fact]
        public void Constructor_ShouldCreateCartItem_WhenDetailsAreValid()
        {
            // Arrange
            var name = "Product 1";
            var quantity = 2;
            var value = 10.5m;
            var productId = Guid.NewGuid();
            var cartClientId = Guid.NewGuid();

            // Act
            var cartItem = new CartItem(name, quantity, value, productId, cartClientId);

            // Assert
            Assert.Equal(name, cartItem.Name);
            Assert.Equal(quantity, cartItem.Quantity);
            Assert.Equal(value, cartItem.Value);
            Assert.Equal(productId, cartItem.ProductId);
            Assert.Equal(cartClientId, cartItem.CartClientId);
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenDetailsAreInvalid()
        {
            // Arrange
            var name = "";
            var quantity = 0;
            var value = 0m;
            var productId = Guid.Empty;
            var cartClientId = Guid.Empty;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                new CartItem(name, quantity, value, productId, cartClientId));
        }

        [Fact]
        public void AddUnities_ShouldIncreaseQuantity_WhenQuantityIsPositive()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act
            cartItem.AddUnities(3);

            // Assert
            Assert.Equal(5, cartItem.Quantity); // 2 + 3 = 5
        }

        [Fact]
        public void AddUnities_ShouldThrowException_WhenQuantityIsZeroOrNegative()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cartItem.AddUnities(0));
            Assert.Throws<InvalidOperationException>(() => cartItem.AddUnities(-1));
        }

        [Fact]
        public void UpdateUnities_ShouldSetQuantity_WhenQuantityIsNonNegative()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act
            cartItem.UpdateUnities(3);

            // Assert
            Assert.Equal(3, cartItem.Quantity);
        }

        [Fact]
        public void UpdateUnities_ShouldThrowException_WhenQuantityIsNegative()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cartItem.UpdateUnities(-1));
        }

        [Fact]
        public void CalculateValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act
            var totalValue = cartItem.CalculateValue();

            // Assert
            Assert.Equal(21m, totalValue); // 2 * 10.5 = 21
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenDetailsAreValid()
        {
            // Arrange
            var cartItem = new CartItem("Product 1", 2, 10.5m, Guid.NewGuid(), Guid.NewGuid());

            // Act
            var isValid = cartItem.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenDetailsAreInvalid()
        {
            // Arrange
            var cartItem = new CartItem("", 0, 0m, Guid.Empty, Guid.Empty);

            // Act
            var isValid = cartItem.IsValid();

            // Assert
            Assert.False(isValid);
        }
    }
}
