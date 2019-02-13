// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Text;

namespace Kit
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class UtilityMisc
    {

        #region ... string

        /// <summary>
        /// 获取当前的 长度 bytes kb mb gb
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLengthString(int length)
        {
            if (length < 1024)
            {
                return Utility.Text.Format("{0} Bytes", length.ToString());
            }

            if (length < 1024 * 1024)
            {
                return Utility.Text.Format("{0} KB", (length / 1024f).ToString("F2"));
            }

            if (length < 1024 * 1024 * 1024)
            {
                return Utility.Text.Format("{0} MB", (length / 1024f / 1024f).ToString("F2"));
            }

            return Utility.Text.Format("{0} GB", (length / 1024f / 1024f / 1024f).ToString("F2"));
        }

        /// <summary>
        /// 获取当前的 长度  mb gb
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetLengthStringMB(long length)
        {
            if (length < 1024 * 1024 * 1024)
            {
                return Utility.Text.Format("{0} MB", (length / 1024f / 1024f).ToString("F2"));
            }
            return Utility.Text.Format("{0} GB", (length / 1024f / 1024f / 1024f).ToString("F2"));
        }

        /// <summary>
        /// 将数字转化为###.##万、百万、千万或###.##亿的结构,当数字小于99999时不转化
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NumToString(long num)
        {
            if (num > 99999999)
            {
                double n = num * 0.00000001f;
                return n.ToString("F2") + "亿";
            }
            if (num > 9999999)
            {
                double n = num * 0.0000001f;
                return n.ToString("F2") + "千万";
            }
            if (num > 999999)
            {
                double n = num * 0.000001f;
                return n.ToString("F2") + "百万";
            }
            if (num > 99999)
            {
                double n = num * 0.0001f;
                return n.ToString("F2") + "万";
            }
            return num.ToString();
        }

        /// <summary>缩减字符串到指定字节长度，超出部分使用“...”代替</summary>
        public static string ReduceStringToLength(string str, int length)
        {
            int byteCount = Encoding.Default.GetByteCount(str);
            if (byteCount > length)
            {
                while (byteCount > length)
                {
                    str = str.Substring(0, str.Length - 1);
                    byteCount = Encoding.Default.GetByteCount(str);
                }
                return str + "...";
            }
            else
                return str;
        }

        /// <summary>
        /// 解析字符串 
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="beginTag"> 开始标签 </param>
        /// <param name="endTag"> 结束标签 </param>
        /// <param name="getTextKeyAction"> 标签内的内容 替换函数
        ///     1. 如果为空则返回 标签开始-标签结束的内容
        ///     2. 如果不为空则 string 传参是标签内的内容 返回值需要返回替换的内容，把文本内容中开始标签和结束标签移除 标签中内容替换返回的文本 
        /// </param>
        /// <returns></returns>
        /// e.g.
        ///    1 = ParseText("Button<L>1</L>","<L>","</L>",null); 结果返回1  标签中的内容反馈
        ///    按钮 = ParseText("<L>1</L>","<L>","</L>",(text)=>{ return "按钮";}); 结果返回1  标签中的内容反馈
        public static string ParseText(string text, string beginTag, string endTag, Func<string, string> getTextKeyAction = null)
        {
            int startIndex = text.IndexOf(beginTag);
            int endIndex = text.IndexOf(endTag);
            string part = null;
            if (startIndex >= 0 && endIndex > startIndex)
            {
                part = text.Substring(startIndex + beginTag.Length, endIndex - startIndex - beginTag.Length);
            }
            if (!string.IsNullOrEmpty(part))
            {
                if (getTextKeyAction != null)
                {
                    string content = getTextKeyAction(part);
                    if (!string.IsNullOrEmpty(content))
                        text = text.Substring(0, startIndex) + content + text.Substring(endIndex + endTag.Length);
                }
                else
                {
                    text = part;
                }
            }
            return text;
        }

        #endregion
         
        /// <summary>字节转兆,B->MB</summary>
        public static float GetMillionFromByte(long bytes)
        {
            return bytes / 1024f / 1024f;
        }

        /// <summary>获取枚举成员总数</summary>
        public static int GetEnumCount<T>()
        {
            if (typeof(T).IsEnum)
                return Enum.GetNames(typeof(T)).GetLength(0);
            return 0;
        }
    }
}