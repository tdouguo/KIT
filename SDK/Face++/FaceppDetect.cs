using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Kit
{
    public class FaceppDetect
    {
        private const string FaceppV3Detect = "https://api-cn.faceplusplus.com/facepp/v3/detect";

        /// <summary>
        /// 人脸识别
        /// </summary>
        /// <param name="img"></param>
        /// <param name="complete"></param>
        /// <returns></returns>
        public IEnumerator PostDetect(byte[] img, RequestComplete complete)
        {
            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("api_key", Utility.Http.EscapeString("api-key"));
            wwwForm.AddField("api_secret", Utility.Http.EscapeString("api-secret"));
            wwwForm.AddField("return_attributes", "emotion");// int  string 类型  详情参数查看 https://console.faceplusplus.com.cn/documents/4888373
            wwwForm.AddBinaryData("image_file", img);// 二进制  类型

            yield return UtilityMisc.RequestByWWW(FaceppV3Detect, wwwForm, complete);
        }


    }

}
