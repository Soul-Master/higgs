using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Higgs.Core.Security
{
    public class HashFunction
    {
        public static string Md5(string msg)
        {
            if (msg == null)
                return null;

            var encoder = new UTF8Encoding();
            var md5Hasher = new MD5CryptoServiceProvider();
            var hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(msg));
            
            return ByteArrayToString(hashedDataBytes);
        }
        
        public static string Sha1(string msg)
        {
            if (msg == null)
                return null;

            var encoder = new UTF8Encoding();
            var sha1Hasher = new SHA1CryptoServiceProvider();
            var hashedDataBytes = sha1Hasher.ComputeHash(encoder.GetBytes(msg));
            
            return ByteArrayToString(hashedDataBytes);
        }
        
        public static string Sha256(string msg)
        {
            if (msg == null)
                return null;

            var encoder = new UTF8Encoding();
            var sha256Hasher = new SHA256Managed();
            var hashedDataBytes = sha256Hasher.ComputeHash(encoder.GetBytes(msg));
            
            return ByteArrayToString(hashedDataBytes);
        }
        
        public static string Sha384(string msg)
        {
            if (msg == null)
                return null;

            var encoder = new UTF8Encoding();
            var sha384Hasher = new SHA384Managed();
            var hashedDataBytes = sha384Hasher.ComputeHash(encoder.GetBytes(msg));
            
            return ByteArrayToString(hashedDataBytes);
        }
        
        public static string Sha512(string msg)
        {
            if (msg == null)
                return null;

            var encoder = new UTF8Encoding();
            var sha512Hasher = new SHA512Managed();
            var hashedDataBytes = sha512Hasher.ComputeHash(encoder.GetBytes(msg));
            
            return ByteArrayToString(hashedDataBytes);
        }

        public static uint CheckSum(Stream s)
        {
            var crc32 = new Crc32();
            crc32.ComputeHash(s);
            
            return crc32.CrcValue;
        }
        
        public static string ByteArrayToString(byte[] inputArray)
        {
            var output = new StringBuilder("");

            foreach (var t in inputArray)
            {
                output.Append(t.ToString("X2"));
            }
            
            return output.ToString();
        }
    }
}
