using System.Text;

namespace TDengineAspCoreClient
{
    public static class MD5
    {
        public static string CreateMD5(this string str)
        {
            // Use input string to calculate MD5 hash
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (var hash in hashBytes)
            {
                sb.Append(hash.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
