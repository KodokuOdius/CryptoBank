using System.Text;
using System.Security.Cryptography;


namespace Crypto {
    internal class _sha256 {
        public static String hash(String text) {
            return Convert.ToHexString(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
