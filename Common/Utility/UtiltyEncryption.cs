// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Text;

namespace Kit
{
    public static partial class Utility
    {
        /// <summary>
        /// 加密解密相关的实用函数。
        /// </summary>
        internal static class Encryption
        {
            private const int QuickEncryptLength = 220;

            /// <summary>
            /// 将 bytes 使用 code 做异或运算的快速版本。
            /// </summary>
            /// <param name="bytes">原始二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <returns>异或后的二进制流。</returns>
            public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
            {
                return GetXorBytes(bytes, code, QuickEncryptLength);
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算。
            /// </summary>
            /// <param name="bytes">原始二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <param name="length">异或计算长度，若小于等于 0，则计算整个二进制流。</param>
            /// <returns>异或后的二进制流。</returns>
            public static byte[] GetXorBytes(byte[] bytes, byte[] code, int length = 0)
            {
                if (bytes == null)
                {
                    return null;
                }

                if (code == null)
                {
                    throw new Exception("Code is invalid.");
                }

                int codeLength = code.Length;
                if (codeLength <= 0)
                {
                    throw new Exception("Code length is invalid.");
                }

                int codeIndex = 0;
                int bytesLength = bytes.Length;
                if (length <= 0 || length > bytesLength)
                {
                    length = bytesLength;
                }

                byte[] result = new byte[bytesLength];
                Buffer.BlockCopy(bytes, 0, result, 0, bytesLength);

                for (int i = 0; i < length; i++)
                {
                    result[i] ^= code[codeIndex++];
                    codeIndex = codeIndex % codeLength;
                }

                return result;
            }

            /// <summary>
            /// 加密 sha1 算法
            /// </summary>
            /// <param name="str"></param>
            /// <returns></returns>
            public static string EncryptToSHA1(string str)
            {
                var buffer = Encoding.UTF8.GetBytes(str);
                var data = System.Security.Cryptography.SHA1.Create().ComputeHash(buffer);

                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString().ToLower();
            }

            /// <summary>
            /// HMAC-SHA1加密算法
            /// </summary>
            /// <param name="secret">密钥</param>
            /// <param name="strOrgData">源文</param>
            /// <returns></returns>
            public static string HmacSha1Sign(string EncryptText, string EncryptKey)
            {
                HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.Default.GetBytes(EncryptKey));
                byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.Default.GetBytes(EncryptText));
                StringBuilder EnText = new StringBuilder();
                foreach (byte Byte in RstRes)
                {
                    EnText.AppendFormat("{0:x2}", Byte);
                }
                return EnText.ToString();
            }
        }
    }
   
}