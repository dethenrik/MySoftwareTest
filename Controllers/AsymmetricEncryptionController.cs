using Microsoft.AspNetCore.Mvc;
using BlazorApp1.Codes;  // Make sure this is the correct namespace

namespace SoftwareTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsymmetricEncryptionController : ControllerBase
    {
        private readonly AsymmetricEncryptionHandler _encryptionHandler;

        // Inject the AsymmetricEncryptionHandler to handle encryption/decryption
        public AsymmetricEncryptionController(AsymmetricEncryptionHandler encryptionHandler)
        {
            _encryptionHandler = encryptionHandler;
        }

        // Encrypt the plain text using the public key
        [HttpPost("encrypt")]
        public IActionResult EncryptText([FromBody] string plainText)
        {
            var encryptedText = _encryptionHandler.Encrypt(plainText);
            return Ok(encryptedText);  // Return the encrypted text as a response
        }

        // Decrypt the encrypted text using the private key
        [HttpPost("decrypt")]
        public IActionResult DecryptText([FromBody] string encryptedText)
        {
            var decryptedText = _encryptionHandler.Decrypt(encryptedText);
            return Ok(decryptedText);  // Return the decrypted text as a response
        }
    }
}
