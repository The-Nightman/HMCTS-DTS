namespace HmctsDts.Server.Interfaces;

public interface ISecurityService
{
    void CreatePassHash(string password, out byte[] hash, out byte[] salt);
    bool ComparePassHash(string passwordInput, byte[] storedPassHash, byte[] salt);
    void ZeroMemory(byte[] array);
    void ZeroMemory(params byte[]?[]? arrays);
}