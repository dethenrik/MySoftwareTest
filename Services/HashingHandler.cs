using System.Security.Cryptography;
using System.Text;

namespace BlazorApp1.Codes;

public class HashingHandler
{
    // MD5
    public dynamic MD5Hashing(dynamic valueToHash) =>
        valueToHash is byte[] 
        ? MD5.Create().ComputeHash(valueToHash) 
        : Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));

    // SHA
    public dynamic SHAHashing(dynamic valueToHash) =>
        valueToHash is byte[] 
        ? SHA256.Create().ComputeHash(valueToHash) 
        : Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(valueToHash)));

    // HMAC
    public dynamic HMACHashing(dynamic valueToHash) =>
        valueToHash is byte[]
        ? new HMACSHA256(Encoding.ASCII.GetBytes("NielsErMinFavoritLærer")).ComputeHash(valueToHash)
        : Convert.ToBase64String(new HMACSHA256(Encoding.ASCII.GetBytes("NielsErMinFavoritLærer")).ComputeHash(valueToHash));

    // PBKDF2
    public dynamic PBKDF2Hashing(dynamic valueToHash) =>
        valueToHash is byte[]
        ? Rfc2898DeriveBytes.Pbkdf2(valueToHash, Encoding.UTF8.GetBytes("230271"), 10, HashAlgorithmName.SHA256, 32)
        : Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(valueToHash, Encoding.UTF8.GetBytes("230271"), 10, HashAlgorithmName.SHA256, 32));

    #region BCrypt

    // Aproach 1
    public string BCryptHashing1(string textToHash) =>
        BCrypt.Net.BCrypt.HashPassword(textToHash);

    public bool BCryptVerifyHashing1(string textToHash, string hashedValue) =>
        BCrypt.Net.BCrypt.Verify(textToHash, hashedValue);


    // Aproach 2
    public string BCryptHashing2(string textToHash) =>
        BCrypt.Net.BCrypt.HashPassword(textToHash, 10, true);

    public bool BCryptVerifyHashing2(string textToHash, string hashedValue) =>
        BCrypt.Net.BCrypt.Verify(textToHash, hashedValue, true);


    // Aproach 3
    public string BCryptHashing3(dynamic textToHash) =>
        BCrypt.Net.BCrypt.HashPassword(textToHash, BCrypt.Net.BCrypt.GenerateSalt(), true, BCrypt.Net.HashType.SHA256);

    public bool BCryptVerifyHashing3(string textToHash, string hashedValue) =>
        BCrypt.Net.BCrypt.Verify(textToHash, hashedValue, true, BCrypt.Net.HashType.SHA256);

    #endregion
}
