using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

#if NETFX_CORE
	using Windows.Security.Cryptography;
	using Windows.Security.Cryptography.Core;
	using Windows.Storage.Streams;
	using BestHTTP.PlatformSupport.IO;

	using FileStream = BestHTTP.PlatformSupport.IO.FileStream;
#elif UNITY_WP8
    using Cryptography = BestHTTP.PlatformSupport.Cryptography;
#else
	using Cryptography = System.Security.Cryptography;
	using FileStream = System.IO.FileStream;
#endif

namespace BestHTTP.Extensions
{
    public static class Extensions
    {
        #region ASCII Encoding (These are required because Windows Phone doesn't supports the Encoding.ASCII class.)

        /// <summary>
        /// On WP8 platform there are no ASCII encoding.
        /// </summary>
        public static string AsciiToString(this byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length);
            foreach (byte b in bytes)
                sb.Append(b <= 0x7f ? (char)b : '?');
            return sb.ToString();
        }

        /// <summary>
        /// On WP8 platform there are no ASCII encoding.
        /// </summary>
        public static byte[] GetASCIIBytes(this string str)
        {
            byte[] result = new byte[str.Length];
            for (int i = 0; i < str.Length; ++i)
            {
                char ch = str[i];
                result[i] = (byte)((ch < (char)0x80) ? ch : '?');
            }

            return result;
        }

        public static void SendAsASCII(this BinaryWriter stream, string str)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                char ch = str[i];

                stream.Write((byte)((ch < (char)0x80) ? ch : '?'));
            }
        }

        #endregion

        #region FileSystem WriteLine function support

        public static void WriteLine(this FileStream fs)
        {
            fs.Write(HTTPRequest.EOL, 0, 2);
        }

        public static void WriteLine(this FileStream fs, string line)
        {
            var buff = line.GetASCIIBytes();
            fs.Write(buff, 0, buff.Length);
            fs.WriteLine();
        }

        public static void WriteLine(this FileStream fs, string format, params object[] values)
        {
            var buff = string.Format(format, values).GetASCIIBytes();
            fs.Write(buff, 0, buff.Length);
            fs.WriteLine();
        }

        #endregion

        #region Other Extensions

        public static string GetRequestPathAndQueryURL(this Uri uri)
        {
            string requestPathAndQuery = uri.GetComponents(UriComponents.PathAndQuery, UriFormat.UriEscaped);

            // http://forum.unity3d.com/threads/best-http-released.200006/page-26#post-2723250
            if (string.IsNullOrEmpty(requestPathAndQuery))
                requestPathAndQuery = "/";

            return requestPathAndQuery;
        }

        public static string[] FindOption(this string str, string option)
        {
            //s-maxage=2678400, must-revalidate, max-age=0
            string[] options = str.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            option = option.ToLower();

            for (int i = 0; i < options.Length; ++i)
                if (options[i].Contains(option))
                    return options[i].Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

            return null;
        }

        public static void WriteArray(this Stream stream, byte[] array)
        {
            stream.Write(array, 0, array.Length);
        }

        #endregion

        #region String Conversions

        public static int ToInt32(this string str, int defaultValue = default(int))
        {
            if (str == null)
                return defaultValue;

            try
            {
                return int.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static long ToInt64(this string str, long defaultValue = default(long))
        {
            if (str == null)
                return defaultValue;

            try
            {
                return long.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static DateTime ToDateTime(this string str, DateTime defaultValue = default(DateTime))
        {
            if (str == null)
                return defaultValue;

            try
            {
                DateTime.TryParse(str, out defaultValue);
                return defaultValue.ToUniversalTime();
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ToStrOrEmpty(this string str)
        {
            if (str == null)
                return String.Empty;

            return str;
        }

        #endregion

        #region MD5 Hashing

        public static string CalculateMD5Hash(this string input)
        {
            return input.GetASCIIBytes().CalculateMD5Hash();
        }

        public static string CalculateMD5Hash(this byte[] input)
        {
#if NETFX_CORE
            var alg = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);
            IBuffer buff = CryptographicBuffer.CreateFromByteArray(input);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
#else
            var hash = Cryptography.MD5.Create().ComputeHash(input);
            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
#endif
        }

        #endregion

        #region Efficient String Parsing Helpers

        internal static string Read(this string str, ref int pos, char block, bool needResult = true)
        {
            return str.Read(ref pos, (ch) => ch != block, needResult);
        }

        internal static string Read(this string str, ref int pos, Func<char, bool> block, bool needResult = true)
        {
            if (pos >= str.Length)
                return string.Empty;

            str.SkipWhiteSpace(ref pos);

            int startPos = pos;

            while (pos < str.Length && block(str[pos]))
                pos++;

            string result = needResult ? str.Substring(startPos, pos - startPos) : null;

            // set position to the next char
            pos++;

            return result;
        }

        internal static string ReadPossibleQuotedText(this string str, ref int pos)
        {
            string result = string.Empty;
            if (str == null)
                return result;

            // It's a quoted text?
            if (str[pos] == '\"')
            {
                // Skip the starting quote
                str.Read(ref pos, '\"', false);

                // Read the text until the ending quote
                result = str.Read(ref pos, '\"');

                // Next option
                str.Read(ref pos, ',', false);
            }
            else
                // It's not a quoted text, so we will read until the next option
                result = str.Read(ref pos, (ch) => ch != ',' && ch != ';');

            return result;
        }

        internal static void SkipWhiteSpace(this string str, ref int pos)
        {
            if (pos >= str.Length)
                return;

            while (pos < str.Length && char.IsWhiteSpace(str[pos]))
                pos++;
        }

        internal static string TrimAndLower(this string str)
        {
            if (str == null)
                return null;

            char[] buffer = new char[str.Length];
            int length = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                char ch = str[i];
                if (!char.IsWhiteSpace(ch) && !char.IsControl(ch))
                    buffer[length++] = char.ToLowerInvariant(ch);
            }

            return new string(buffer, 0, length);
        }

        internal static char? Peek(this string str, int pos)
        {
            if (pos < 0 || pos >= str.Length)
                return null;

            return str[pos];
        }

        #endregion

        #region Specialized String Parsers

        //public, max-age=2592000
        internal static List<HeaderValue> ParseOptionalHeader(this string str)
        {
            List<HeaderValue> result = new List<HeaderValue>();

            if (str == null)
                return result;

            int idx = 0;

            // process the rest of the text
            while (idx < str.Length)
            {
                // Read key
                string key = str.Read(ref idx, (ch) => ch != '=' && ch != ',').TrimAndLower();
                HeaderValue qp = new HeaderValue(key);

                if (str[idx - 1] == '=')
                    qp.Value = str.ReadPossibleQuotedText(ref idx);

                result.Add(qp);
            }

            return result;
        }

        //deflate, gzip, x-gzip, identity, *;q=0
        internal static List<HeaderValue> ParseQualityParams(this string str)
        {
            List<HeaderValue> result = new List<HeaderValue>();

            if (str == null)
                return result;

            int idx = 0;
            while (idx < str.Length)
            {
                string key = str.Read(ref idx, (ch) => ch != ',' && ch != ';').TrimAndLower();

                HeaderValue qp = new HeaderValue(key);

                if (str[idx - 1] == ';')
                {
                    str.Read(ref idx, '=', false);
                    qp.Value = str.Read(ref idx, ',');
                }

                result.Add(qp);
            }

            return result;
        }

        #endregion

        #region Buffer Filling

        /// <summary>
        /// Will fill the entire buffer from the stream. Will throw an exception when the underlying stream is closed.
        /// </summary>
        public static void ReadBuffer(this Stream stream, byte[] buffer)
        {
            int count = 0;

            do
            {
                int read = stream.Read(buffer, count, buffer.Length - count);

                if (read <= 0)
                    throw ExceptionHelper.ServerClosedTCPStream();

                count += read;
            } while (count < buffer.Length);
        }

        #endregion

        #region MemoryStream

        public static void WriteAll(this MemoryStream ms, byte[] buffer)
        {
            ms.Write(buffer, 0, buffer.Length);
        }

        public static void WriteString(this MemoryStream ms, string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            ms.WriteAll(buffer);
        }

        public static void WriteLine(this MemoryStream ms)
        {
            ms.WriteAll(HTTPRequest.EOL);
        }

        public static void WriteLine(this MemoryStream ms, string str)
        {
            ms.WriteString(str);
            ms.WriteLine();
        }

        #endregion
    }

    public static class ExceptionHelper
    {
        public static Exception ServerClosedTCPStream()
        {
            return new Exception("TCP Stream closed unexpectedly by the remote server");
        }
    }
}