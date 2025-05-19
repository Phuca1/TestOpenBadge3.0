using NSec.Cryptography;

namespace EricTest.Services
{
    public class KeyService
    {
        public (string publicKey, string privateKey) GenerateEd25519KeyPair()
        {
            var algorithm = SignatureAlgorithm.Ed25519;
            var creationParam = new KeyCreationParameters
            {
                ExportPolicy = KeyExportPolicies.AllowPlaintextExport,
            };

            using var key = Key.Create(algorithm, creationParam);
            var publicKey = Convert.ToBase64String(key.Export(KeyBlobFormat.RawPublicKey));
            var privateKey = Convert.ToBase64String(key.Export(KeyBlobFormat.RawPrivateKey));

            return (publicKey, privateKey);
        }
    }
}
