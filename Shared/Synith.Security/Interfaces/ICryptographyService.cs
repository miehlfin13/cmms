namespace Synith.Security.Interfaces;

public interface ICryptographyService
{
    void SetKey(string key);
    void SetIV(string iv);

    string Decrypt(string value);
    string Encrypt(string value);
    string Hash(string value);

    bool IsPasswordValid(string password, string hash);
}
