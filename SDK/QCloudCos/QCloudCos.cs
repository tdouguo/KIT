using BestHTTP;
using Kit.Runtime;
using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Kit
{
    /// <summary>
    /// cos 控制类 提供上传功能
    /// </summary>
    public class QCloudCos
    {
        private const float COS_KEY_TIMEOUT = 60 * 10; // 10 分钟重新获取
        private CosKeyDataModel cosKeyData;  //     cosKeyData = WebRequestResult.JsonToObject<CosKeyDataModel>("cos_key_dict");

        #region ... API

        /// <summary>
        /// 检查 cos key有效期
        /// </summary>
        /// <param name="complete"></param>
        public void CheckCosKey(UnityAction complete = null)
        {
            if (cosKeyData != null && !cosKeyData.IsNull)
            {
                long tmpCosKeyTime = UtilityTime.GetTimeStampByUtcNow(); // cosKeyData.expiredTime;
                long nowCosKeyTime = UtilityTime.GetTimeStampByUtcNow();
                if ((tmpCosKeyTime - nowCosKeyTime) > COS_KEY_TIMEOUT) // 密钥在有效时间内
                {
                    if (complete != null)
                    {
                        complete();
                    }
                    return;
                }
            }
            // TODO：发送获取 cos  key 请求   key从服务器获取 
        }

        #region  文件处理  Put

        /// <summary>
        /// put 对象
        /// </summary>
        /// <param name="url">远程地址</param>
        /// <param name="key">文件路径 无/ </param>
        /// <param name="file"></param>
        public void PutObject(string url, string key, byte[] file, UnityAction<HTTPRequest, HTTPResponse> complete = null)
        {
            CheckCosKey(() =>
            {
                string _url = url + key;

                Debug.LogFormat("[QCloudCos].PutObject url  {0} . ", _url);

                HTTPRequest request = new HTTPRequest(new Uri(_url), HTTPMethods.Put, (req, resp) =>
                {
                    if (resp.StatusCode == 200)
                    {
                        Debug.LogFormat("[QCloudCos].PutObject complete  {0} . ", _url);
                    }
                    else
                    {
                        Debug.LogErrorFormat("[QCloudCos].PutObject code error   url :''{0}' , code : '{1}' , data :'{2}' . ", _url, resp.StatusCode, resp.DataAsText);
                    }
                    if (complete != null)
                    {
                        complete.Invoke(req, resp);
                    }
                });
                string sign = GetSign("put", "/" + key, "", "");
                request.AddHeader("x-cos-security-token", cosKeyData.sessionToken);
                request.AddHeader("Authorization", sign);
                request.AddBinaryData("file", file);
                request.OnUploadProgress = OnUploadProgressCallback;
                request.Send();
            });
        }

        private void OnUploadProgressCallback(HTTPRequest request, long downloaded, long downloadLength)
        {
            Debug.LogFormat("on upload progress " + request.CurrentUri + " " + request.State + " " + downloaded + " " + downloadLength);
        }

        #endregion

        #endregion

        #region ... Sign Tool

        public string GetSign(string methods, string url, string parameters, string headers)
        {
            /* 
             q-sign-algorithm=sha1
             &q-ak=[SecretID]
             &q-sign-time=[SignTime]
             &q-key-time=[KeyTime]
             &q-header-list=[SignedHeaderList]
             &q-url-param-list=[SignedParameterList]
             &q-signature=[Signature]
             */

            var now = UtilityTime.GetTimeStampByUtcNow();
            var exp = cosKeyData.expiredTime;
            string singtime = now + ";" + exp;

            string signKey = Utility.Encryption.HmacSha1Sign(singtime, cosKeyData.tmpSecretKey);
            //Debug.Log("计算结果signKey " + signKey);
            string httpString = methods + "\n" + url + "\n" + parameters + "\n" + headers + "\n";
            //Debug.Log("计算结果 " + httpString);

            string stringToSign = "sha1\n" + singtime + "\n" + Utility.Encryption.EncryptToSHA1(httpString) + "\n";
            //Debug.Log("计算结果 " + stringToSign);

            string signature = Utility.Encryption.HmacSha1Sign(stringToSign, signKey);
            //Debug.Log("计算结果 " + signature);

            StringBuilder authorization = new StringBuilder();
            authorization.Append("q-sign-algorithm=sha1");

            authorization.Append("&q-ak=");
            authorization.Append(cosKeyData.tmpSecretId);

            authorization.Append("&q-sign-time=");
            authorization.Append(singtime);

            authorization.Append("&q-key-time=");
            authorization.Append(singtime);

            authorization.Append("&q-header-list=");
            authorization.Append(parameters);

            authorization.Append("&q-url-param-list=");
            authorization.Append(headers);

            authorization.Append("&q-signature=");
            authorization.Append(signature);

            string authStr = authorization.ToString();
            Debug.Log("sign  " + authStr);

            return authStr;
        }

        #endregion

    }
}