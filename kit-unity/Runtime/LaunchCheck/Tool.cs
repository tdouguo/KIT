using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace TeamPackage
{
    public static class Tool
    {
        public static void QuitApp()
        {
            Debug.Log("QuitApp ...");
            Application.Quit(0);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        /// <summary>
        /// 网络可达性
        /// </summary> 
        /// <returns></returns>
        public static bool IsNetworkReachability()
        {
            switch (Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    //Log("当前使用的是：WiFi，请放心更新！");
                    return true;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    //Log("当前使用的是移动网络，是否继续更新？");
                    return true;
                case NetworkReachability.NotReachable:
                default:
                    //Log("当前没有联网，请您先联网后再进行操作！");
                    return false;
            }
        }

        public static UnityWebRequestAsyncOperation RequestText(string url, Action<string> onCompleted)
        {
            try
            {
                UnityWebRequest request = UnityWebRequest.Get(url);
                UnityWebRequestAsyncOperation ao = request.SendWebRequest();
                ao.completed += (_ao) =>
                {
                    if (string.IsNullOrEmpty(request.error))
                    {
                        // string contents = Encoding.UTF32.GetString(request.downloadHandler.data);
                        string contents = request.downloadHandler.text;
                        onCompleted?.Invoke(contents);
                    }
                    else
                    {
                        Debug.LogError("UnityWebRequestUtil.RequestText " + url + " err :" + request.error);
                        onCompleted?.Invoke(string.Empty);
                    }

                    request.Abort();
                };
                return ao;
            }
            catch (Exception e)
            {
                Debug.LogError("UnityWebRequestUtil.SendRequest " + url + " Exception :" + e);
                return null;
            }
        }


        #region 时间

        public static long GetTimeStampByDateTime(DateTime time, bool isMilliseconds = false)
        {
            TimeSpan ts = time - new DateTime(1970, 1, 1, 0, 0, 0, 0);

            if (isMilliseconds)
            {
                return Convert.ToInt64(ts.TotalMilliseconds);
            }
            else
            {
                return Convert.ToInt64(ts.TotalSeconds);
            }
        }


        // https://laoguicom.top/doc/473/
        private const string NetTime_苏宁 = "https://f.m.suning.com/api/ct.do"; // 13位 时间戳 currentTime
        private const string NetTime_苏宁2 = "http://quan.suning.com/getSysTime.do"; // 获取时间

        private const string NetTime_京东 = "http://a.jd.com/ajax/queryServerData.html";
        // private const string NetTime_PDD = "https://api.pinduoduo.com/api/server/_stm"; // 时间戳 13位结果
        // private const string NetTime_国美 = "https://im-platform.gome.com.cn/im-platform/system/serverTime.json?appId=gomeplus_pro";
        // private const string NetTime_QQ = "http://cgi.im.qq.com/cgi-bin/cgi_svrtime";


        public static void GetNetTime_QQ(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://vv6.video.qq.com/checktime";
            RequestText(url, (text) =>
            {
                long timestamp = 0;

                if (!string.IsNullOrEmpty(text))
                {
                    string regexText = "/\\d+(.\\d+)?/g)[2]";
                    Match match = Regex.Match(text, regexText);
                    if (match.Success)
                    {
                        string result = match.Groups[0].Value;
                        if (long.TryParse(result, out timestamp))
                        {
                            succeedCallback?.Invoke(timestamp);
                            return;
                        }
                    }
                }

                failedCallback?.Invoke();
            });
        }

        public static void GetNetTime_魅族(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://book.meizu.com/gettime.js";
            RequestText(url, (text) =>
            {
                long timestamp = 0;
                if (!string.IsNullOrEmpty(text))
                {
                    string result = text.Substring(16, 10); // 时间戳 10位结果
                    if (long.TryParse(result, out timestamp))
                    {
                        timestamp *= 1000;
                        succeedCallback?.Invoke(timestamp);
                        return;
                    }
                }

                failedCallback?.Invoke();
            });
        }

        public static void GetNetTime_TimeIS(Action<long> succeedCallback, Action failedCallback)
        {
            string url =
                "https://time.is/t/?zh.0.347.2464.0p.480.43d.1574683214663.1594044507830.";
            // 时间戳 13位结果
            RequestText(url, (text) =>
            {
                long timestamp = 0;
                if (!string.IsNullOrEmpty(text))
                {
                    string result = text.Substring(0, 13);
                    if (long.TryParse(result, out timestamp))
                    {
                        succeedCallback?.Invoke(timestamp);
                        return;
                    }
                }

                failedCallback?.Invoke();
            });
        }

        public static void GetNetTime_小米有品(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://tptm.hd.mi.com/gettimestamp";
            // 时间戳 10位结果
            RequestText(url, (text) =>
            {
                long timestamp = 0;
                if (!string.IsNullOrEmpty(text))
                {
                    string time = text.Split("=")[1];
                    if (long.TryParse(time, out timestamp))
                    {
                        timestamp *= 1000;
                        succeedCallback?.Invoke(timestamp);
                        return;
                    }
                }

                failedCallback?.Invoke();
            });
        }

        public static void GetNetTime_物美多点(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://appapis.dmall.com/static/queryUserCheckInfo.jsonp";
            RequestText(url, (text) =>
            {
                if (!string.IsNullOrEmpty(text))
                {
                    string time = text.Substring(63, 13);
                    if (long.TryParse(time, out long timestamp))
                    {
                        succeedCallback?.Invoke(timestamp);
                        return;
                    }
                }

                failedCallback?.Invoke();
            });
        }

        #region 时间戳 淘宝

        public static void GetNetTime_淘宝(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://api.m.taobao.com/rest/api3.do?api=mtop.common.getTimestamp";
            // {"api":"mtop.common.getTimestamp","v":"*","ret":["SUCCESS::接口调用成功"],"data":{"t":"1695273519496"}}

            RequestText(url, (text) =>
            {
                if (!string.IsNullOrEmpty(text))
                {
                    NetTime_淘宝_Result data = JsonUtility.FromJson<NetTime_淘宝_Result>(text);
                    long timestamp = data.data.t;
                    succeedCallback?.Invoke(timestamp);
                }
                else
                {
                    failedCallback?.Invoke();
                }
            });
        }

        struct NetTime_淘宝_Result
        {
            public string api;
            public string v;
            public string[] ret;
            public NetTime_淘宝_Data data;
        }

        struct NetTime_淘宝_Data
        {
            public long t;
        }

        #endregion

        #region 时间戳 腾讯

        public static void GetNetTime_腾讯(Action<long> succeedCallback, Action failedCallback)
        {
            string url = "https://vv.video.qq.com/checktime?otype=json";
            // QZOutputJson={"s":"o","t":1695273527,"ip":"221.217.228.166","pos":"---","rand":"0FDcQ9SEYYLcsg_JvXAD2w=="};
            RequestText(url, (text) =>
            {
                if (!string.IsNullOrEmpty(text))
                {
                    string begin = "QZOutputJson=";
                    int len = text.Length - 13 - 1;
                    string result = text.Substring(13, len);
                    NetTime_腾讯_Data data = JsonUtility.FromJson<NetTime_腾讯_Data>(result);
                    long timestamp = data.t * 1000;
                    succeedCallback?.Invoke(timestamp);
                }
                else
                {
                    failedCallback?.Invoke();
                }
            });
        }

        struct NetTime_腾讯_Data
        {
            public string s;
            public long t;
            public string ip;
            public string pos;
            public string rand;
        }

        #endregion

        #endregion

        private delegate void GetNetTimeCallback(Action<long> succeedCallback, Action failedCallback);

        private static readonly List<GetNetTimeCallback> GetNetTimeFuncList = new List<GetNetTimeCallback>()
        {
            GetNetTime_TimeIS,
            GetNetTime_QQ,
            GetNetTime_魅族,
            GetNetTime_小米有品,
            GetNetTime_物美多点,
            GetNetTime_淘宝,
            GetNetTime_腾讯,
        };

        public static void GetNetTime(Action<long> callback)
        {
            if (GetNetTimeFuncList.Count == 0)
            {
                callback?.Invoke(0);
                return;
            }

            GetNetTimeFuncList[0](callback, () =>
            {
                GetNetTimeFuncList.RemoveAt(0);
                GetNetTime(callback);
            });
        }
    }
}