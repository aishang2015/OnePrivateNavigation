using System;
using System.Security.Cryptography;
using System.Text;

namespace OnePrivateNavigation.Data.Helpers
{
    /// <summary>
    /// 提供常用哈希算法的辅助类
    /// </summary>
    public class HashHelper
    {
        /// <summary>
        /// 计算字符串的MD5哈希值
        /// </summary>
        /// <param name="input">要计算哈希值的字符串</param>
        /// <returns>32位小写MD5哈希字符串</returns>
        public static string ComputeMD5(string input)
        {
            using var md5 = MD5.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 计算字符串的SHA256哈希值
        /// </summary>
        /// <param name="input">要计算哈希值的字符串</param>
        /// <returns>64位小写SHA256哈希字符串</returns>
        public static string ComputeSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 使用salt计算字符串的SHA256哈希值
        /// </summary>
        /// <param name="input">要计算哈希值的字符串</param>
        /// <param name="salt">盐值</param>
        /// <returns>64位小写SHA256哈希字符串</returns>
        public static string ComputeSHA256WithSalt(string input, string salt)
        {
            using var sha256 = SHA256.Create();
            var inputBytes = Encoding.UTF8.GetBytes(input + salt);
            var hashBytes = sha256.ComputeHash(inputBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        /// <summary>
        /// 计算字节数组的SHA256哈希值
        /// </summary>
        /// <param name="input">要计算哈希值的字节数组</param>
        /// <returns>哈希后的字节数组</returns>
        public static byte[] ComputeSHA256(byte[] input)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(input);
        }

        /// <summary>
        /// 生成指定长度的随机盐值
        /// </summary>
        /// <param name="length">盐值长度</param>
        /// <returns>Base64编码的盐值字符串</returns>
        public static string GenerateSalt(int length = 32)
        {
            var salt = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }
    }
}
