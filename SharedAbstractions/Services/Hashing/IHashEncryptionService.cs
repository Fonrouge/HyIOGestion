namespace Shared.Services
{
    public interface IHashEncryptionService
    {
        string Hash(string input);
        bool Verify(string input, string hashed);
    }
}
