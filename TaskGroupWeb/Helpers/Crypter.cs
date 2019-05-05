using System;
using System.Security.Cryptography;
using System.Text;

namespace TaskGroupWeb.Helpers
{
    public class Crypter
    {
        public static string GetMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "").ToUpper();
            }
        }
    }
}
