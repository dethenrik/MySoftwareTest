using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace BlazorApp1.Codes
{
    public class AsymmetricEncryptionHandler
    {
        private readonly RSA _rsa;
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        private const string PublicKeyPath = "public_key.pem";
        private const string PrivateKeyPath = "private_key.pem";

        public AsymmetricEncryptionHandler()
        {
            _rsa = RSA.Create();

            // Load keys if they exist; otherwise, generate new ones
            if (File.Exists(PublicKeyPath) && File.Exists(PrivateKeyPath))
            {
                LoadKeys();
            }
            else
            {
                GenerateAndSaveKeys();
            }
        }

        private void GenerateAndSaveKeys()
        {
            // Generate a new key pair
            var keyPair = _rsa.ExportParameters(true);

            // Separate public and private keys
            _publicKey = new RSAParameters
            {
                Modulus = keyPair.Modulus,
                Exponent = keyPair.Exponent
            };

            _privateKey = keyPair;

            // Save keys to .pem files
            SaveKey(PublicKeyPath, _rsa.ExportRSAPublicKey());
            SaveKey(PrivateKeyPath, _rsa.ExportRSAPrivateKey());
        }

        private void SaveKey(string path, byte[] keyData)
        {
            File.WriteAllText(path, Convert.ToBase64String(keyData));
        }

        private void LoadKeys()
        {
            // Load the public key
            var publicKeyData = Convert.FromBase64String(File.ReadAllText(PublicKeyPath));
            _rsa.ImportRSAPublicKey(publicKeyData, out _);
            _publicKey = _rsa.ExportParameters(false);

            // Load the private key
            var privateKeyData = Convert.FromBase64String(File.ReadAllText(PrivateKeyPath));
            _rsa.ImportRSAPrivateKey(privateKeyData, out _);
            _privateKey = _rsa.ExportParameters(true);
        }

        // Method to encrypt text using the public key
        public string Encrypt(string plainText)
        {
            _rsa.ImportParameters(_publicKey); // Use public key for encryption
            var data = Encoding.UTF8.GetBytes(plainText);
            var encryptedData = _rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Convert.ToBase64String(encryptedData);
        }

        // Method to decrypt text using the private key
        public string Decrypt(string encryptedText)
        {
            _rsa.ImportParameters(_privateKey); // Use private key for decryption
            var data = Convert.FromBase64String(encryptedText);
            var decryptedData = _rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
            return Encoding.UTF8.GetString(decryptedData);
        }
    }
}
