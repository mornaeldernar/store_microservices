using Basket.Service.Models;

namespace Basket.Tests.Domain;
public class CustomerBasketTests
{
    [Fact]
    public void GivenAnEmptyCustomerBasket_WhenCallingAddBasketProduct_ThenProductAddedToBasket()
    {
        // Given
        var product = new Service.Models.BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new Service.Models.CustomerBasket { CustomerId = "1" };
        // When
        customerBasket.AddBasketProduct(product);

        // Then
        Assert.Contains(product, customerBasket.Products);
    }
    [Fact]
    public void GivenCustomerBasketWithProduct_WhenCallingAddBasketProductWithExistingProduct_ThenBasketUpdated()
    {
        // Given
        var product = new Service.Models.BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new Service.Models.CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);
        var updatedProduct = product with
        {
            Quantity = 2
        };

        // When
        customerBasket.AddBasketProduct(updatedProduct);

        // Then
        Assert.Contains(updatedProduct, customerBasket.Products);
        Assert.Equal(updatedProduct.Quantity, customerBasket.Products.First().Quantity);
    }
    [Fact]
    public void GivenCustomerBasketWithProduct_WhenCallingRemoveBasketProduct_ThenProductSuccessfullyRemoved()
    {
        // Given
        var product = new Service.Models.BasketProduct("1", "Test Name", 9.99M);
        var customerBasket = new Service.Models.CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);

        // When
        customerBasket.RemoveBasketProduct("1");

        // Then
        Assert.Empty(customerBasket.Products);
    }

    [Fact]
    public void GivenCustomerBasket_WhenAddingProduct_ThenBasketTotalCalculatedCorrectly()
    {
        // Given
        const decimal basketTotal = 19.98M;

        // When
        var product = new BasketProduct("1", "Test Name", 9.99M,2);
        var customerBasket = new CustomerBasket { CustomerId = "1" };
        customerBasket.AddBasketProduct(product);

        // Then
        Assert.Equal(basketTotal, customerBasket.BasketTotal);
    }
}