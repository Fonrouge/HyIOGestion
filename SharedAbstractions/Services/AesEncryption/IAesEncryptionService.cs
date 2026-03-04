namespace Shared.Services
{
    public interface IAesEncryptionService
    {
        byte[] Decrypt(byte[] data);

        byte[] Encrypt(byte[] data);
        
    }
}
