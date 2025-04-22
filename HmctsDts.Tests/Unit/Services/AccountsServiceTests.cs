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
    private RegisterUserDto _validUserRegister;
    private RegisterUserDto _duplicateUserRegister;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _testPepper = "TestPepper"u8.ToArray();
        _accountsService = new AccountsService(_mockUserRepository, _testPepper);
        _validUserRegister = new RegisterUserDto
        {
            Name = "John Doe",
            Email = "john.doe@test.com",
            Password = "Password1!"
        };
        _duplicateUserRegister = new RegisterUserDto()
        {
            Name = "Jane Doe",
            Email = "john.doe@test.com",
            Password = "Password1!"
        };
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
        User? capturedUser = null;
        await _mockUserRepository.CreateUser(Arg.Do<User>(u => capturedUser = u));

        // Act
        var result = await _accountsService.RegisterNewCaseWorker(_validUserRegister);

        // Assert
        await _mockUserRepository.Received(1).CreateUser(Arg.Any<User>());
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.True);
            Assert.That(capturedUser, Is.Not.Null);
        });
        Assert.Multiple(() =>
        {
            Assert.That(capturedUser.Name, Is.EqualTo(_validUserRegister.Name));
            Assert.That(capturedUser.Email, Is.EqualTo(_validUserRegister.Email));
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
        _mockUserRepository.GetUserByEmail(Arg.Any<string>())
            .Returns(
                x => Task.FromResult<User?>(null),
                x => Task.FromResult<User?>(new User
                {
                    Email = _duplicateUserRegister.Email,
                    Name = _duplicateUserRegister.Name,
                    StaffId = "EJD-CTS-1234",
                    Hash = "testHash"u8.ToArray(),
                    Salt = "testSalt"u8.ToArray(),
                })
            );

        // Act
        var resultPass = await _accountsService.RegisterNewCaseWorker(_validUserRegister);
        var resultFail = await _accountsService.RegisterNewCaseWorker(_duplicateUserRegister);

        // Assert
        await _mockUserRepository.Received(1).CreateUser(Arg.Any<User>());
        Assert.Multiple(() =>
        {
            Assert.That(resultPass, Is.True);
            Assert.That(resultFail, Is.False);
        });
    }

    /// <summary>
    /// Verifies that the Login method successfully returns a user with the correct credentials.
    /// </summary>
    [Test]
    [Category("Unit")]
    [Category("AccountsService")]
    public async Task Login_WithValidCredentials_ReturnsAuthenticatedUser()
    {
        // Arrange
        var loginObj = new LoginDto
        {
            Email = _validUserRegister.Email,
            Password = _validUserRegister.Password
        };

        await _accountsService.RegisterNewCaseWorker(_validUserRegister);

        // Act
        var result = await _accountsService.Login(loginObj);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(_validUserRegister.Name));
            Assert.That(result.StaffId, Does.Match(@"^EJD-CTS-\d{4}$"));
        });
    }

    /// <summary>
    /// Verifies that the Login method returns null when provided with invalid credentials.
    /// </summary>
    [Test]
    [Category("Unit")]
    [Category("AccountsService")]
    public async Task Login_WithInvalidCredentials_ReturnsNull()
    {
        // Arrange
        var loginObj = new LoginDto
        {
            Email = _validUserRegister.Email,
            Password = $"Wrong{_validUserRegister.Password}"
        };

        await _accountsService.RegisterNewCaseWorker(_validUserRegister);

        // Act
        var result = await _accountsService.Login(loginObj);

        // Assert
        Assert.That(result, Is.Null);
    }
}