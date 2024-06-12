using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Synith.Security.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Synith.Security.Services;
public class CryptographyService : ICryptographyService
{
    private string Key { get; set; } = "bLZvdQ7ZtRGXIDcGvJ5F6LcO/dxqoayZ8pzSVGeDLTc=";
    private string IV { get; set; } = "7PSUjJfD+xex8otst/i7tA==";

    public void SetKey(string key) => Key = key;
    public void SetIV(string iv) => IV = iv;

    public CryptographyService(IConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration["Encryption:Key"]))
        {
            Key = configuration["Encryption:Key"]!;
        }

        if (!string.IsNullOrEmpty(configuration["Encryption:IV"]))
        {
            IV = configuration["Encryption:IV"]!;
        }
    }

    public string Encrypt(string value)
    {
        try
        {
            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(IV);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream ms = new();
            using (CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write))
            {
                using StreamWriter sw = new(cs);
                sw.Write(value);
            }

            return Convert.ToBase64String(ms.ToArray());
        }
        catch
        {
            throw new CryptographicException();
        }
    }

    public string Decrypt(string value)
    {
        try
        {
            byte[] encryptedBytes = Convert.FromBase64String(value);

            using Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(Key);
            aes.IV = Convert.FromBase64String(IV);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new(encryptedBytes);
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);

            return sr.ReadToEnd();
        }
        catch
        {
            throw new CryptographicException();
        }
    }

    public string Hash(string value)
    {
        string salt = BCryptNet.GenerateSalt(12);
        return BCryptNet.HashPassword(value, salt);
    }

    public bool IsPasswordValid(string password, string hash)
    {
        return BCryptNet.Verify(password, hash);
    }
}
