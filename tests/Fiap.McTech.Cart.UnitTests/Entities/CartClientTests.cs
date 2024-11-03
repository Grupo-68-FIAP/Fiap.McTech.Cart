using Fiap.McTech.Cart.Api.Entities;
using Xunit; 

namespace UnitTests.Entities
{
    public class CartClientTests
    {
        [Fact]
        public void AddItem_ShouldAddNewItem_WhenItemIsNotInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();

            // Act
            cartClient.AddItem("Item 1", 2, 10.5m, productId);

            // Assert
            Assert.Single(cartClient.Items);
            Assert.Equal("Item 1", cartClient.Items[0].Name);
            Assert.Equal(2, cartClient.Items[0].Quantity);
            Assert.Equal(10.5m, cartClient.Items[0].Value);
        }

        [Fact]
        public void AddItem_ShouldIncreaseQuantity_WhenItemAlreadyExistsInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();
            cartClient.AddItem("Item 1", 2, 10.5m, productId);

            // Act
            cartClient.AddItem("Item 1", 3, 10.5m, productId);

            // Assert
            Assert.Single(cartClient.Items);
            Assert.Equal(5, cartClient.Items[0].Quantity); // 2 + 3 = 5
        }

        [Fact]
        public void UpdateItemQuantity_ShouldUpdateQuantity_WhenItemExistsInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();
            cartClient.AddItem("Item 1", 2, 10.5m, productId);

            // Act
            cartClient.UpdateItemQuantity(productId, 5);

            // Assert
            Assert.Equal(5, cartClient.Items[0].Quantity);
        }

        [Fact]
        public void UpdateItemQuantity_ShouldThrowException_WhenItemDoesNotExistInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => cartClient.UpdateItemQuantity(productId, 5));
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItem_WhenItemExistsInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();
            cartClient.AddItem("Item 1", 2, 10.5m, productId);

            // Act
            cartClient.RemoveItem(productId);

            // Assert
            Assert.Empty(cartClient.Items);
        }

        [Fact]
        public void RemoveItem_ShouldDoNothing_WhenItemDoesNotExistInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();

            // Act
            cartClient.RemoveItem(productId);

            // Assert
            Assert.Empty(cartClient.Items);
        }

        [Fact]
        public void CalculateTotalValue_ShouldReturnCorrectTotal_WhenItemsExistInCart()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            cartClient.AddItem("Item 1", 2, 10.5m, Guid.NewGuid()); // 2 * 10.5 = 21
            cartClient.AddItem("Item 2", 1, 15m, Guid.NewGuid());   // 1 * 15 = 15

            // Act
            var totalValue = cartClient.CalculateTotalValue();

            // Assert
            Assert.Equal(36m, totalValue); // 21 + 15 = 36
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCartClientIsValid()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            cartClient.AddItem("Item 1", 1, 10m, Guid.NewGuid());

            // Act
            var isValid = cartClient.IsValid();

            // Assert
            Assert.True(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenClientIdIsEmpty()
        {
            // Arrange
            var cartClient = new CartClient(Guid.Empty);

            // Act
            var isValid = cartClient.IsValid();

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAnItemIsInvalid()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            var cartClient = new CartClient(clientId);
            var productId = Guid.NewGuid();

            var invalidItem = new CartItem("", 0, 0m, productId, cartClient.Id);
            cartClient.Items.Add(invalidItem);

            // Act
            var isValid = cartClient.IsValid();

            // Assert
            Assert.False(isValid);
        }
    }
}
