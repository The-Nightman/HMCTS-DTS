using System.Security.Cryptography;
using System.Text;
using HmctsDts.Server.Interfaces;
using Konscious.Security.Cryptography;

namespace HmctsDts.Server.Security;

public class SecurityService(byte[] pepper) : ISecurityService
{
    public void CreatePassHash(string password, out byte[] hash, out byte[] salt)
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

        ZeroMemory(argonHash);
    }

    public bool ComparePassHash(string passwordInput, byte[] storedPassHash, byte[] salt)
    {
        using var argon2Id = new Argon2id(Encoding.UTF8.GetBytes(passwordInput))
        {
            Salt = salt,
            DegreeOfParallelism = 1,
            Iterations = 2,
            MemorySize = 19456,
        };
        var argonHash = argon2Id.GetBytes(128);

        using var hmac = new HMACSHA512(pepper);

        var inputHashResult = hmac.ComputeHash(argonHash);

        var result = CryptographicOperations.FixedTimeEquals(storedPassHash, inputHashResult);

        ZeroMemory(argonHash, inputHashResult);

        return result;
    }

    public void ZeroMemory(byte[] array)
    {
        CryptographicOperations.ZeroMemory(array.AsSpan());
    }

    public void ZeroMemory(params byte[]?[]? arrays)
    {
        if (arrays == null) return;

        foreach (var array in arrays)
        {
            if (array != null)
                CryptographicOperations.ZeroMemory(array.AsSpan());
        }
    }
}