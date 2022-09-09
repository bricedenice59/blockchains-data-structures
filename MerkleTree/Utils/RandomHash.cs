using System.Security.Cryptography;

namespace BlockChainDataStructures
{
    public class RandomHash
    {
        public static string GetFromRandomString()
        {
            byte[] random = RandomNumberGenerator.GetBytes(1024);
            return Sha256Hash.Get(random);
        }
    }
}

