using HmctsDts.Server.DTOs;
using HmctsDts.Server.Entities;
using HmctsDts.Server.Interfaces;
using HmctsDts.Server.Security;
using HmctsDts.Server.Services;
using NSubstitute;

namespace HmctsDts.Tests.Unit.Services;

public class AccountsServiceTests
{
    private IUserRepository _mockUserRepository;
    private AccountsService _accountsService;
    private byte[] _testPepper;
    private RegisterUserDto _validUserRegister;
    private SecurityService _securityService;
    private const string StaffIdRegex = @"^EJD-CTS-\d{4}$";

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = Substitute.For<IUserRepository>();
        _testPepper = "TestPepper"u8.ToArray();
        _securityService = new SecurityService(_testPepper);
        _accountsService = new AccountsService(_mockUserRepository, _securityService);
        _validUserRegister = new RegisterUserDto
        {
            Name = "John Doe",
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
            Assert.That(capturedUser?.Name, Is.EqualTo(_validUserRegister.Name));
            Assert.That(capturedUser?.Email, Is.EqualTo(_validUserRegister.Email));
            Assert.That(capturedUser?.StaffId, Does.Match(StaffIdRegex));
            Assert.That(capturedUser?.Hash, Is.Not.Null);
            Assert.That(capturedUser?.Salt, Is.Not.Null);
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
        var duplicateUser = new RegisterUserDto()
        {
            Name = "Jane Doe",
            Email = _validUserRegister.Email,
            Password = "Password1!"
        };

        _mockUserRepository.GetUserByEmail(Arg.Any<string>())
            .Returns(
                x => Task.FromResult<User?>(null),
                x => Task.FromResult<User?>(new User
                {
                    Email = duplicateUser.Email,
                    Name = duplicateUser.Name,
                    StaffId = "EJD-CTS-1234",
                    Hash = "testHash"u8.ToArray(),
                    Salt = "testSalt"u8.ToArray()
                })
            );

        // Act
        var resultPass = await _accountsService.RegisterNewCaseWorker(_validUserRegister);
        var resultFail = await _accountsService.RegisterNewCaseWorker(duplicateUser);

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

        _securityService.CreatePassHash(_validUserRegister.Password, out var hash, out var salt);

        _mockUserRepository.GetUserByEmail(Arg.Any<string>())
            .Returns(x => Task.FromResult<User?>(new User
                {
                    Email = _validUserRegister.Email,
                    Name = _validUserRegister.Name,
                    StaffId = "EJD-CTS-1234",
                    Hash = hash,
                    Salt = salt
                })
            );

        // Act
        var result = await _accountsService.Login(loginObj);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Name, Is.EqualTo(_validUserRegister.Name));
            Assert.That(result?.StaffId, Does.Match(StaffIdRegex));
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

        _securityService.CreatePassHash(_validUserRegister.Password, out var hash, out var salt);

        _mockUserRepository.GetUserByEmail(Arg.Any<string>())
            .Returns(x => Task.FromResult<User?>(new User
                {
                    Email = _validUserRegister.Email,
                    Name = _validUserRegister.Name,
                    StaffId = "EJD-CTS-1234",
                    Hash = hash,
                    Salt = salt
                })
            );

        // Act
        var result = await _accountsService.Login(loginObj);

        // Assert
        Assert.That(result, Is.Null);
    }
}