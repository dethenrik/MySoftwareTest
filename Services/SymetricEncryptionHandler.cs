using Microsoft.AspNetCore.DataProtection;

namespace BlazorApp1.Codes
{
    public class SymetricEncryptionHandler
    {
        private readonly IDataProtector _key;

        public SymetricEncryptionHandler(IDataProtectionProvider dataProtectionProvider)
        {
            // Ensure that the protector name ("TodoProtectionKey") is consistent
            _key = dataProtectionProvider.CreateProtector("TodoProtectionKey");
        }

        public string Encrypt(string valueToEncrypt) =>
            _key.Protect(valueToEncrypt);

        public string Decrypt(string valueToDecrypt) =>
            _key.Unprotect(valueToDecrypt);
    }
}
