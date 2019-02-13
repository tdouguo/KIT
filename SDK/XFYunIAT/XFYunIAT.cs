using BestHTTP;
using Kit.Runtime;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Kit
{

    /// <summary>
    /// 科大讯飞服务   语音识别
    ///        注： 如此项服务需要下客户端使用 则提交科大讯飞工单 解除白名单限制
    /// </summary>
    public class XFYunIAT
    {
        private const string Host = "http://api.xfyun.cn/v1/service/v1/iat";
        private const string AppId = "";
        private const string AppKey = "";

        /// <summary>
        /// 音频编码，可选值：raw（未压缩的pcm或wav格式）、speex（speex格式）、speex-wb（宽频speex格式）
        /// </summary>
        private const string Aue = "raw";

        /// <summary>
        /// 发送语音识别
        /// </summary>
        /// <param name="audio">AudioClip 音频</param>
        /// <param name="complete">回调结果</param>
        /// <param name="engineType"> 普通话(sms16k),普通话(sms8k),英语(sms-en8k),英语(sms-en16k) </param>
        public void PostIAT(byte[] audio, UnityAction<string> complete, EngineType engineType = EngineType.SmsEn16k)
        {
            if (audio == null || audio.Length == 0)
            {
                Debug.LogErrorFormat("[XFYunIAT].PostIAT  audio error null or count=0. ");
                if (complete != null)
                {
                    complete.Invoke(string.Empty);
                }
                return;
            }
            string curTime = UtilityTime.GetTimeStampStrByUtcNow();

            string param = "{\"aue\":\"" + Aue + "\"" + ",\"engine_type\":\"" + GetEngineType(engineType) + "\"}";
            byte[] bytedata = Encoding.ASCII.GetBytes(param);
            string x_param = Convert.ToBase64String(bytedata);
            string result = string.Format("{0}{1}{2}", AppKey, curTime, x_param);
            string X_checksum = Md5(result);

            HTTPRequest request = new HTTPRequest(new Uri(Host), HTTPMethods.Post, (req, resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    Debug.LogFormat("[XFYunIAT].PostIAT complete .");
                    Debug.LogFormat("返回结果 '{0}' .", resp.DataAsText);
                    if (complete != null)
                    {
                        complete.Invoke(resp.DataAsText);
                    }
                }
                else
                {

                    Debug.LogErrorFormat("[XFYunIAT].PostIAT code error  code : '{0}' , data :'{1}'  ,  msg : '{2}'. ", resp.StatusCode, resp.DataAsText, resp.Message);
                    if (complete != null)
                    {
                        complete.Invoke(string.Empty);
                    }
                }
            });

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("charset", "utf-8");

            request.AddHeader("X-Appid", AppId);
            request.AddHeader("X-CurTime", curTime);
            request.AddHeader("X-Param", x_param);
            request.AddHeader("X-CheckSum", X_checksum);

            string audioStr = Convert.ToBase64String(audio);

            request.AddField("audio", audioStr);
            request.Send();
        }

        /// <summary>
        /// 引擎类型，可选值：sms16k（16k采样率普通话音频）、sms8k（8k采样率普通话音频）等
        /// 普通话(sms16k),普通话(sms8k),英语(sms-en8k),英语(sms-en16k)
        /// </summary>
        /// <param name="engineType"></param>
        /// <returns></returns>
        private string GetEngineType(EngineType engineType)
        {
            switch (engineType)
            {
                case EngineType.Sms16k:
                    return "sms16k";
                case EngineType.Sms8k:
                    return "sms8k";
                case EngineType.SmsEn8k:
                    return "sms-en8k";
                case EngineType.SmsEn16k:
                    return "sms-en16k";
                default:
                    return string.Empty;
            }
        }

        private string Md5(string s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();
            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }
            return ret.PadLeft(32, '0');
        }
    }
}
