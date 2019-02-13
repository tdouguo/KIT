using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kit
{
    /// <summary>
    /// 
    /// </summary>
    public class CosKeyDataModel
    {
        // 2038 年10 月 此处有BUG  会超出int 类型范围
        public int expiredTime; // cos 参数有效时间 
        public Dictionary<string, string> credentials;

        public string sessionToken { get { return IsNull ? string.Empty : credentials["sessionToken"]; } }
        public string tmpSecretId { get { return IsNull ? string.Empty : credentials["tmpSecretId"]; } }   // 临时Id
        public string tmpSecretKey { get { return IsNull ? string.Empty : credentials["tmpSecretKey"]; } } // 临时密钥

        public bool IsNull
        {
            get
            {
                return expiredTime == default(double) && (credentials == null || credentials.Count == 0);
            }
        }
    } 
}