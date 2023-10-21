using System.Security.Cryptography;
using System.Text;

namespace CyptoWallet.Helper
{
    public class PassEncryptHelper
    {
        public static string CreatePass(string password, string email)
        {

            var salt = CreateSalt(email);
            string saltAndPwd = String.Concat(password, salt);
            var sha256 = SHA256.Create();
            var encoding = new ASCIIEncoding();
            var stream = Array.Empty<byte>();
            var sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(saltAndPwd));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }
            return sb.ToString();
        }
        private static string CreateSalt(string email)
        {
            // Convierte el entero en una cadena
            string salt = email.ToString();

            byte[] saltBytes;
            string saltStr;

            // Convierte la cadena en un arreglo de bytes
            saltBytes = ASCIIEncoding.ASCII.GetBytes(salt);

            long XORED = 0x00;

            foreach (int x in saltBytes)
            {
                XORED = XORED ^ x;
            }

            Random rand = new Random(Convert.ToInt32(XORED));
            saltStr = rand.Next().ToString();
            saltStr += rand.Next().ToString();
            saltStr += rand.Next().ToString();
            saltStr += rand.Next().ToString();

            return saltStr;
        }
    }
}
