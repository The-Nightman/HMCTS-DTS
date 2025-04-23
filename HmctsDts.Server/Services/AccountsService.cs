using HmctsDts.Server.DTOs;
using HmctsDts.Server.Entities;
using HmctsDts.Server.Interfaces;

namespace HmctsDts.Server.Services;

public class AccountsService(IUserRepository userRepository, ISecurityService securityService) : IAccountsService
{
    public async Task<bool> RegisterNewCaseWorker(RegisterUserDto registerUserDto)
    {
        securityService.CreatePassHash(registerUserDto.Password, out var hash, out var salt);

        var newCaseWorker = new User
        {
            Name = registerUserDto.Name,
            Email = registerUserDto.Email,
            StaffId = await CreateStaffId(registerUserDto.Name),
            Hash = hash,
            Salt = salt
        };

        // We check for an existing user here so that we allow the non-database operations to persist so that timing
        // attacks are made significantly harder by minimizing the ms difference between creation or conflict
        var existingUser = await userRepository.GetUserByEmail(registerUserDto.Email);

        if (existingUser != null)
            return false;

        try
        {
            await userRepository.CreateUser(newCaseWorker);
        }
        finally
        {
            securityService.ZeroMemory(hash, salt);
        }

        return true;
    }

    public async Task<StaffDataDto?> Login(LoginDto loginDto)
    {
        var existingUser = await userRepository.GetUserByEmail(loginDto.Email);

        // We create default byte arrays so that we have some dummy data to compare against to keep constant time
        var hash = existingUser?.Hash ?? new byte[64];
        var salt = existingUser?.Salt ?? new byte[64];

        var isValid = securityService.ComparePassHash(loginDto.Password, hash, salt);

        securityService.ZeroMemory(hash, salt);

        if (existingUser == null || !isValid) return null;

        securityService.ZeroMemory(existingUser.Hash, existingUser.Salt);

        return new StaffDataDto
        {
            Name = existingUser.Name,
            StaffId = existingUser.StaffId
        };
    }

    private async Task<string> CreateStaffId(string name)
    {
        const int maxAttempts = 100;
        var attempts = 0;

        while (attempts < maxAttempts)
        {
            var rand = new Random();

            var nameIdentifier = string.Concat(
                name.Split(" ").First()[..1],
                name.Split(" ").Last()[..1]
            );

            var uid = rand.Next(0, 9999).ToString("D4");

            var staffId = string.Concat("E", nameIdentifier, "-CTS-", uid);

            var existingStaffId = await userRepository.GetUserByStaffId(staffId);
            if (existingStaffId == null)
            {
                return staffId;
            }

            attempts++;
        }

        throw new InvalidOperationException("Failed to generate a unique staff ID after 100 attempts.");
    }
}