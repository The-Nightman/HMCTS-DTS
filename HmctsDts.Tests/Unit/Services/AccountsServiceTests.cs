using HmctsDts.Server.DTOs;
using HmctsDts.Server.Entities;
using HmctsDts.Server.Interfaces;
using HmctsDts.Server.Services;
using NSubstitute;

namespace HmctsDts.Tests.Unit.Services;

public class AccountsServiceTests
{
    private IUserRepository _mockUserRepository;
    private IAccountsService _accountsService;
    private byte[] _testPepper;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _testPepper = "TestPepper"u8.ToArray();
        _accountsService = new AccountsService(_mockUserRepository, _testPepper);
    }

    /// <summary>
    /// Verifies that the RegisterNewCaseWorker method creates a new user with the correct properties.
    /// </summary>
    [Test]
    [Category("Unit")]
    [Category("AccountsService")]
    public async Task RegisterNewCaseWorker_CreatesUser()
    {
        // Arrange
        var userObj = new RegisterUserDto
        {
            Name = "John Doe",
            Email = "john.doe@test.com",
            Password = "Password1!"
        };

        User? capturedUser = null;
        await _mockUserRepository.CreateUser(Arg.Do<User>(u => capturedUser = u));

        // Act
        await _accountsService.RegisterNewCaseWorker(userObj);

        // Assert
        await _mockUserRepository.Received(1).CreateUser(Arg.Any<User>());
        
        Assert.That(capturedUser, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(capturedUser.Name, Is.EqualTo(userObj.Name));
            Assert.That(capturedUser.Email, Is.EqualTo(userObj.Email));
            Assert.That(capturedUser.StaffId, Does.Match(@"^EJD-CTS-\d{4}$"));
            Assert.That(capturedUser.Hash, Is.Not.Null);
            Assert.That(capturedUser.Salt, Is.Not.Null);
        });
    }
}