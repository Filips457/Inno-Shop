using FluentAssertions;
using Moq;
using ProductApplication.Exceptions;
using ProductApplication.Interfaces;
using ProductApplication.Services;
using ProductDomain.Model;

namespace UnitTests.Tests.ProductServ;

public class ProductServiceUnitTests
{
    private readonly Mock<IProductRepository> repoMock;

    private readonly ProductService service;

    public ProductServiceUnitTests()
    {
        var products = new List<Product>
        {
            new Product
            {
                Id = 1,
                Name = "Ноутбук",
                Description = "Игровой ноутбук с RTX 4060",
                Price = 1499.99m,
                CreationTime = DateTime.UtcNow.AddDays(-10),
                IsActive = true,
                UserId = 1
            },
            new Product
            {
                Id = 2,
                Name = "Мышь",
                Description = "Беспроводная мышь Logitech",
                Price = 29.99m,
                CreationTime = DateTime.UtcNow.AddDays(-5),
                IsActive = false,
                UserId = 2
            },
            new Product
            {
                Id = 3,
                Name = "Монитор",
                Description = "27\" IPS 144Hz",
                Price = 299.99m,
                CreationTime = DateTime.UtcNow.AddDays(-2),
                IsActive = true,
                UserId = 1
            }
        };

        repoMock = new Mock<IProductRepository>();
        repoMock.Setup(r => r.GetAllProducts()).ReturnsAsync(products);
        repoMock.Setup(r => r.GetActiveProducts()).ReturnsAsync(products.Where(p => p.IsActive == true).ToList());
        repoMock.Setup(r => r.GetProductById(It.IsAny<int>())).ReturnsAsync(
            (int id) => products.FirstOrDefault(p => p.Id == id));

        service = new ProductService(repoMock.Object);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnAll()
    {
        var res = await service.GetAllProducts();

        res.Should().HaveCount(3);

        res[0].Name.Should().Be("Ноутбук");
        res[1].Name.Should().Be("Мышь");
        res[2].Name.Should().Be("Монитор");
    }

    [Fact]
    public async Task GetActiveProducts_ShouldReturnOnlyActive()
    {
        var res = await service.GetActiveProducts();

        repoMock.Verify(r => r.GetActiveProducts(), Times.Once);
        res.Should().HaveCount(2);
        res[0].Name.Should().Be("Ноутбук");
        res[1].Name.Should().Be("Монитор");
    }

    [Fact]
    public async Task GetProductById_ShouldReturn()
    {
        var res = await service.GetProductById(1);

        res.Should().NotBeNull();

        res.Name.Should().Be("Ноутбук");
    }

    [Fact]
    public async Task GetProductById_ShouldThrowExc()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetProductById(-3));
    }

    [Fact]
    public async Task SetProductActive_SetTrue()
    {
        await service.SetProductsActive(1, true);
        await service.SetProductsActive(2, true);

        repoMock.Verify(r => r.SetProductsActive(1, true), Times.Once);
        repoMock.Verify(r => r.SetProductsActive(2, true), Times.Once);
    }

    [Fact]
    public async Task SetUserActive_SetFalse()
    {
        await service.SetProductsActive(1, false);
        await service.SetProductsActive(2, false);

        repoMock.Verify(r => r.SetProductsActive(1, false), Times.Once);
        repoMock.Verify(r => r.SetProductsActive(2, false), Times.Once);
    }
}