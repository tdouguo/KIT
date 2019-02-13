// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;

namespace Kit
{
    /// <summary>
    ///  
    /// </summary>
    public static class UtilityGPS
    {
        #region ...定位功能
        /// <summary>
        /// 获取用户定位，数组内为{经度,纬度},获取成功后关闭定位
        /// </summary>
        /// <returns>{经度,纬度}</returns>
        public static float[] GetLatitudeAndLongitude()
        {
            float[] fs = null;
            if (Input.location.status == LocationServiceStatus.Running)
            {
                fs = new float[] { Input.location.lastData.longitude, Input.location.lastData.latitude };
                Input.location.Stop();
            }
            return fs;
        }

        /// <summary>
        /// 开启GPS 定位
        /// </summary>
        /// <param name="desiredAccuracyInMeters"></param>
        /// <param name="updateDistanceInMeters"></param>
        /// <returns></returns>
        public static IEnumerator StartGPSAc(float desiredAccuracyInMeters, float updateDistanceInMeters)
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.LogError("无法获取到定位，需要设置GPS权限");
                yield break;
            }
            Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }

            if (maxWait < 1)
            {
                Debug.LogWarning("初始化GPS超时");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.LogError("启用GPS定位失败");
                yield break;
            }
            else
            {
                Debug.Log("开启定位成功");
            }
        }

        /// <summary>
        /// 获取大致定位地址
        /// </summary>
        /// <param name="ua"></param>
        /// <returns></returns>
        public static IEnumerator GetLocationAc(UnityAction<object> ua)
        {
            float[] lngAndlat = GetLatitudeAndLongitude();
            int count = 0;
            while (lngAndlat == null)
            {
                if (count > 20)
                {
                    Debug.LogError("获取经纬度失败");
                    yield break;
                }
                count++;
                yield return new WaitForSecondsRealtime(3);
                lngAndlat = GetLatitudeAndLongitude();
            }
            //WWW www = new WWW("http://api.map.baidu.com/geocoder/v2/?pois=0&location=30.548883,104.053483&output=json&ak=mW4aUCRrRZIMM8mKWXdlVHq8pOdYEt2o");
            // 百度location参数说明：纬度在前，经度在后
            WWW www = new WWW("http://api.map.baidu.com/geocoder/v2/?location=" + lngAndlat[1] + "," + lngAndlat[0] + "&output=json&pois=0&ak=mW4aUCRrRZIMM8mKWXdlVHq8pOdYEt2o");

            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                try
                {
                    Debug.LogFormat("GPS 信息  : {0} .", Regex.Unescape(www.text));
                    ua(www.text);
                    // TODO: 返回GPS 具体内容信息， 一下解析为示例，

                    // JsonData jd = JsonMapper.ToObject(Regex.Unescape(www.text));
                    // JsonData addressComponentJd = jd["result"]["addressComponent"];
                    // ua(new string[] {
                    // lngAndlat[0].ToString(),
                    // lngAndlat[1].ToString(),
                    // addressComponentJd.TryGetString("province"),
                    // addressComponentJd.TryGetString("city"),
                    // jd["result"].TryGetString("cityCode")
                    //  });
                }
                catch
                {
                    Debug.LogError("获取定位信息失败");
                }
            }
        }
        #endregion
    }
}
