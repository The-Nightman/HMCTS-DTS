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
    public async Task RegisterNewCaseWorker_CreatesUser_ReturnsTrue()
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
        var result = await _accountsService.RegisterNewCaseWorker(userObj);

        // Assert
        await _mockUserRepository.Received(1).CreateUser(Arg.Any<User>());
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(capturedUser, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(capturedUser.Name, Is.EqualTo(userObj.Name));
            Assert.That(capturedUser.Email, Is.EqualTo(userObj.Email));
            Assert.That(capturedUser.StaffId, Does.Match(@"^EJD-CTS-\d{4}$"));
            Assert.That(capturedUser.Hash, Is.Not.Null);
            Assert.That(capturedUser.Salt, Is.Not.Null);
        });
    }

    /// <summary>
    /// Verifies that the RegisterNewCaseWorker method rejects creating users with duplicate emails and returns false.
    /// </summary>
    [Test]
    [Category("Unit")]
    [Category("AccountsService")]
    public async Task RegisterNewCaseWorker_DuplicateEmail_ReturnsFalse()
    {
        // Arrange
        var userObj = new RegisterUserDto
        {
            Name = "John Doe",
            Email = "john.doe@test.com",
            Password = "Password1!"
        };
        var dupeUserObj = new RegisterUserDto
        {
            Name = "Jane Doe",
            Email = "john.doe@test.com",
            Password = "Password1!"
        };

        _mockUserRepository.GetUserByEmail(Arg.Any<string>())
            .Returns(
                x => Task.FromResult<User?>(null),
                x => Task.FromResult<User?>(new User
                {
                    Email = dupeUserObj.Email,
                    Name = dupeUserObj.Name,
                    StaffId = "EJD-CTS-1234",
                    Hash = "testHash"u8.ToArray(),
                    Salt = "testSalt"u8.ToArray(),
                })
            );

        // Act
        var resultPass = await _accountsService.RegisterNewCaseWorker(userObj);
        var resultFail = await _accountsService.RegisterNewCaseWorker(dupeUserObj);

        // Assert
        await _mockUserRepository.Received(1).CreateUser(Arg.Any<User>());
        Assert.Multiple(() =>
        {
            Assert.That(resultPass, Is.True);
            Assert.That(resultFail, Is.False);
        });
    }
}