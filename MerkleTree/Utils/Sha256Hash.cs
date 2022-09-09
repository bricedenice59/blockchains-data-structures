using System.Security.Cryptography;
using System.Text;

namespace BlockChainDataStructures
{
    public class Sha256Hash
    {
        public static string Get(byte[] input)
        {
            ReadOnlySpan<byte> ros = new(input);
            Span<byte> des = new(new byte[32]);

            int bytesWritten;
            bool hasSucceeded = SHA256.TryHashData(ros, des, out bytesWritten);
            if (!hasSucceeded) return String.Empty;

            StringBuilder sBuilder = new();
            for (int i = 0; i < bytesWritten; i++)
            {
                sBuilder.Append(des[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}

