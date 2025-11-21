//using FluentAssertions;
//using Moq;
//using UserApplication.DTOs;
//using UserApplication.Interfaces;
//using UserApplication.Services;
//using UserDomain.Models;

//namespace UnitTests;

////TODO: дописать тесты и интеграционное тестирование
//public class UserServiceUnitTests
//{
//    private readonly Mock<IUserRepository> mockRep;
//    private readonly Mock<IPasswordHasher> mockPass;
//    private readonly UserService service;

//    public UserServiceUnitTests()
//    {
//        mockRep = new Mock<IUserRepository>();
//        mockPass = new Mock<IPasswordHasher>();
//        service = new UserService(mockRep.Object, mockPass.Object);
//    }

//    [Fact]
//    public async Task CreateUser_ShouldThrow_WhenEmailInvalid()
//    {
//        var dto = new UserRequestDTO { Email = "invalid_email" };

//        Func<Task> act = () => service.CreateUser(dto);

//        await act.Should().ThrowAsync<Exception>()
//            .WithMessage("Incorrect email!");
//    }

//    [Fact]
//    public async Task CreateUser_ShouldReturnUserDTO_WhenEmailValid()
//    {
//        var dto = new UserDTO
//        {
//            Email = "user@example.com",
//            Name = "Test",
//            UserRole = Role.SimpleUser,
//            IsActive = true
//        };

//        mockRep.Setup(r => r.CreateUser(It.IsAny<User>()))
//            .ReturnsAsync(new UserRequestDTO
//            {
//                Email = dto.Email,
//                Name = dto.Name,
//                UserRole = dto.UserRole,
//                IsActive = dto.IsActive
//            });

//        var result = await service.CreateUser(dto);

//        result.Should().NotBeNull();
//        result.Email.Should().Be(dto.Email);
//        result.Id.Should().Be(1);
//    }
//}