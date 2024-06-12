using Castle.Core.Configuration;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Synith.Security.Test.Services;

public class CryptographyServiceUnitTest
{
    private const string _plainText = "yourStrong(!)Password";
    private const string _encrypted = "YH6ctpojh6fwLI7+9+nkiXiYikwp1u2B/R52LORmUH0=";

    [Fact]
    public void Encrypt_ReturnsEncryptedValue()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        string actual = cryptography.Encrypt(_plainText);
        actual.Should().Be(_encrypted);
    }

    [Fact]
    public void Encrypt_InvalidKey_ThrowsError()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetKey("Invalid Key");
        Func<string> action = () => cryptography.Encrypt(_plainText);
        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public void Encrypt_InvalidIV_ThrowsError()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetIV("Invalid IV");
        Func<string> action = () => cryptography.Encrypt(_plainText);
        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public void Decrypt_ReturnsDecryptedValue()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        string actual = cryptography.Decrypt(_encrypted);
        actual.Should().Be(_plainText);
    }

    [Fact]
    public void Decrypt_InvalidKey_ThrowsError()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetKey("Invalid Key");
        Func<string> action = () => cryptography.Decrypt(_encrypted);
        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public void Decrypt_InvalidIV_ThrowsError()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetIV("Invalid IV");
        Func<string> action = () => cryptography.Decrypt(_encrypted);
        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public void Hash_ReturnsHashedValue()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        string actual = cryptography.Hash(_plainText);
        bool isValid = BCrypt.Net.BCrypt.Verify(_plainText, actual);
        actual.Should().NotBe(_plainText);
        isValid.Should().BeTrue();

        cryptography.SetKey("VMa5ZnzoAX0sdiF6ggKKccgvNCzbVdScjgRQZVnaxb0=");
        cryptography.SetIV("26wajp0c0Ukb0u8OVs2rLg==");
        string s = cryptography.Encrypt("admin");
        string s2 = cryptography.Encrypt("admin@synith.com");
    }

    [Fact]
    public void SetKey_Invalid_ThrowsException()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetKey(Convert.ToBase64String([]));
        Action action = () => cryptography.Encrypt("");
        action.Should().Throw<CryptographicException>();
    }

    [Fact]
    public void SetIV_Invalid_ThrowsException()
    {
        Mock<MsConfig.IConfiguration> configMock = new();
        CryptographyService cryptography = new(configMock.Object);
        cryptography.SetIV(Convert.ToBase64String([]));
        Action action = () => cryptography.Encrypt("");
        action.Should().Throw<CryptographicException>();
    }
}