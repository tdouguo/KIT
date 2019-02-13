
// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;

namespace Kit
{
    /// <summary>
    /// 
    /// </summary>
    public static class UtilityTime
    {
        /// <summary>时间戳转DateTime</summary>
        public static DateTime GetDateTimeByTimeStamp(long timeStamp)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
            return startTime.AddSeconds(timeStamp);
        }

        /// <summary>DateTime转时间戳</summary>
        public static long GetTimeStampByDateTime(DateTime time)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 获取当前时间戳long类型的
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStampByUtcNow()
        {
            return GetTimeStampByDateTime(DateTime.UtcNow);
        }

        /// <summary>
        /// 获取当前时间戳的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStampStrByUtcNow()
        {
            return GetTimeStampByDateTime(DateTime.UtcNow).ToString();
        }

        /// <summary> 时间转分秒结构 MM:SS </summary>
        public static string GetTimeStrToMS(int time)
        {
            int minute1 = time / 600;
            time -= minute1 * 600;

            int minute2 = time / 60;
            time -= minute2 * 60;

            int second1 = time / 10;
            time -= second1 * 10;

            return Utility.Text.Format("{0}{1}:{2}{3}", minute1, minute2, second1, time);
        }

    }
}