using System.Security.Cryptography;
using System.Text;
using HmctsDts.Server.DTOs;
using HmctsDts.Server.Entities;
using HmctsDts.Server.Interfaces;
using Konscious.Security.Cryptography;

namespace HmctsDts.Server.Services;

public class AccountsService(IUserRepository userRepository, byte[] pepper) : IAccountsService
{
    public async Task<bool> RegisterNewCaseWorker(RegisterUserDto registerUserDto)
    {
        CreatePassHash(registerUserDto.Password, out var hash, out var salt, pepper);

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

        await userRepository.CreateUser(newCaseWorker);

        return true;
    }

    private static void CreatePassHash(string password, out byte[] hash, out byte[] salt, byte[] pepper)
    {
        salt = RandomNumberGenerator.GetBytes(64);

        using var argon2Id = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            DegreeOfParallelism = 1,
            Iterations = 2,
            MemorySize = 19456,
        };
        var argonHash = argon2Id.GetBytes(128);

        // According to the OWASP password cheat sheet, we can enhance the security of our password hashing
        // by using a pepper. This value should be stored in the secrets.json file and not in the database.
        // We need to add the pepper as a singleton service in the ApplicationServiceExtensions.cs file.
        // See: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#peppering
        using var hmac = new HMACSHA512(pepper);

        hash = hmac.ComputeHash(argonHash);

        CryptographicOperations.ZeroMemory(argonHash.AsSpan());
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