using FluentAssertions;
using Moq;
using UnitTests.FakeHttpHandler;
using UserApi.Exceptions;
using UserApplication.Interfaces;
using UserApplication.Services;
using UserDomain.Models;

namespace UnitTests.Tests.UserServ;

public class UserServiceUnitTests
{
    private readonly Mock<IUserRepository> repoMock;
    private readonly Mock<IPasswordHasher> hasherMock;
    private readonly Mock<IHttpClientFactory> httpFactoryMock;

    private readonly UserService service;

    public UserServiceUnitTests()
    {
        var users = new List<User>
        {
            new User { Id = 1, Name = "Art", IsActive = true },
            new User { Id = 2, Name = "Anton", IsActive = false },
            new User { Id = 3, Name = "Alina", IsActive = true },
        };

        repoMock = new Mock<IUserRepository>();
        repoMock.Setup(r => r.GetAllUsers()).ReturnsAsync(users);
        repoMock.Setup(r => r.GetActiveUsers()).ReturnsAsync(users.Where(u => u.IsActive == true).ToList());
        repoMock.Setup(r => r.GetUserById(It.IsAny<int>())).ReturnsAsync(
            (int id) => users.FirstOrDefault(u => u.Id == id));

        hasherMock = new Mock<IPasswordHasher>();

        var httpClient = new HttpClient(new FakeHandler())
        {
            BaseAddress = new Uri("https://localhost:7176/api/Product/")
        };

        httpFactoryMock = new Mock<IHttpClientFactory>();
        httpFactoryMock
            .Setup(f => f.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);

        service = new UserService(repoMock.Object, hasherMock.Object, httpFactoryMock.Object);
    }

    [Fact]
    public async Task GetActiveUsers_ShouldReturnAll()
    {
        var res = await service.GetAllUsers();

        res[0].Name.Should().Be("Art");
        res[1].Name.Should().Be("Anton");
        res[2].Name.Should().Be("Alina");
    }

    [Fact]
    public async Task GetActiveUsers_ShouldReturnOnlyActive()
    {
        var res = await service.GetActiveUsers();

        repoMock.Verify(r => r.GetActiveUsers(), Times.Once);
        res.Should().HaveCount(2);
        res[0].Name.Should().Be("Art");
        res[1].Name.Should().Be("Alina");
    }

    [Fact]
    public async Task GetUserById_ShouldReturn()
    {
        var res = await service.GetUserById(1);

        res.Should().NotBeNull();

        res.Name.Should().Be("Art");
    }

    [Fact]
    public async Task GetUserById_ShouldThrowExc()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => service.GetUserById(-3));
    }

    [Fact]
    public async Task SetUserActive_SetTrue()
    {
        await service.SetUserActive(1, true);
        await service.SetUserActive(2, true);

        repoMock.Verify(r => r.SetUserActive(1, true), Times.Once);
        repoMock.Verify(r => r.SetUserActive(2, true), Times.Once);
    }

    [Fact]
    public async Task SetUserActive_SetFalse()
    {
        await service.SetUserActive(1, false);
        await service.SetUserActive(2, false);

        repoMock.Verify(r => r.SetUserActive(1, false), Times.Once);
        repoMock.Verify(r => r.SetUserActive(2, false), Times.Once);
    }

    [Fact]
    public async Task SetUserActive_ShouldThrowExc()
    {
        await Assert.ThrowsAsync<NotFoundException>(() => service.SetUserActive(-3, true));
    }
}