// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    ///  
    /// </summary> 
    public static class UnityExtends
    {
        #region ... Transform

        #region ... Find
        ///// <summary>
        ///// 广度 查找子物体 [待实现]
        ///// </summary>
        ///// <param name="trans"></param>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //public static Transform FindByBFS(this Transform trans, string name)
        //{ 
        //    return null;
        //}

        /// <summary>
        /// 深度 查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static Transform FindByDFS(this Transform parent, string childName)
        {
            if (parent.name == childName)
            {
                return parent;
            }
            if (parent.childCount < 1) { return null; }
            Transform tf = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform _tf = parent.GetChild(i);
                tf = FindByDFS(_tf, childName);
                if (tf != null)
                {
                    break;
                }
            }
            return tf;
        }

        /// <summary>
        /// 深度 查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindByDFS<T>(this Transform parent, string childName) where T : Component
        {
            if (parent.name == childName)
            {
                return parent.GetComponent<T>();
            }
            if (parent.childCount < 1) { return null; }
            Transform tf = null;
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform _tf = parent.GetChild(i);
                tf = FindByDFS(_tf, childName);
                if (tf != null)
                {
                    break;
                }
            }
            return tf == null ? null : tf.GetComponent<T>();
        }

        /// <summary>
        /// 深度 查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static GameObject FindByDFS(this GameObject parent, string childName)
        {
            Transform tf = FindByDFS(parent.transform, childName);
            return tf ? tf.gameObject : null;
        }

        /// <summary>
        /// 深度 查找子物体
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="childName"></param>
        /// <returns></returns>
        public static T FindByDFS<T>(this GameObject parent, string childName) where T : Component
        {
            return parent.transform.FindByDFS<T>(childName);
        }
        #endregion

        /// <summary> 设置目标物体下所有子物体的显隐状态 </summary>
        public static void SetAllChildrenActive(this Transform trans, bool active)
        {
            for (int i = 0; i < trans.childCount; ++i)
                trans.GetChild(i).gameObject.SetActive(active);
        }

        /// <summary> 设置目标物体和目标物体下所有子物体的layer </summary>
        public static void SetAllChildrenLayer(this Transform tf, int layer)
        {
            tf.gameObject.layer = layer;
            for (int i = 0; i < tf.childCount; i++)
                tf.GetChild(i).gameObject.layer = layer;
        }

        /// <summary> 设置Canvas的GraphicRaycaster的状态 </summary>
        public static void SetCanvasRarcaster(this Transform tf, bool isEnable)
        {
            GraphicRaycaster gr = tf.GetComponent<GraphicRaycaster>();
            if (gr)
                gr.enabled = isEnable;
            else
                Debug.LogErrorFormat("SetCanvasRarcaster  '{0}'  not  GraphicRaycaster .", tf.name);
        }
        public static void InitRectTransform(this RectTransform rectTf)
        {
            rectTf.anchorMin = Vector2.zero;
            rectTf.anchorMax = Vector2.one;
            rectTf.anchoredPosition = Vector2.zero;
            rectTf.rotation = Quaternion.identity;
            rectTf.localScale = Vector3.one;
            rectTf.sizeDelta = Vector2.zero;
        }
        public static void InitTransform(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        public static void InitLocalTransform(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = Vector3.one;
        }

        #region ... Position

        /// <summary>
        /// 设置绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.position;
            v.z = newValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.x += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.y += deltaValue;
            transform.position = v;
        }

        /// <summary>
        /// 增加绝对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值增量。</param>
        public static void AddPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.position;
            v.z += deltaValue;
            transform.position = v;
        }

        #endregion

        #region ... LocalPosition

        /// <summary>
        /// 设置相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetLocalPositionX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.x = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetLocalPositionY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.y = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 设置相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetLocalPositionZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localPosition;
            v.z = newValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 x 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值。</param>
        public static void AddLocalPositionX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.x += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 y 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值。</param>
        public static void AddLocalPositionY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.y += deltaValue;
            transform.localPosition = v;
        }

        /// <summary>
        /// 增加相对位置的 z 坐标。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 坐标值。</param>
        public static void AddLocalPositionZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localPosition;
            v.z += deltaValue;
            transform.localPosition = v;
        }

        #endregion

        #region ... LocalScale

        /// <summary>
        /// 设置相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">x 分量值。</param>
        public static void SetLocalScaleX(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.x = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">y 分量值。</param>
        public static void SetLocalScaleY(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.y = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 设置相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="newValue">z 分量值。</param>
        public static void SetLocalScaleZ(this Transform transform, float newValue)
        {
            Vector3 v = transform.localScale;
            v.z = newValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 x 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">x 分量增量。</param>
        public static void AddLocalScaleX(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.x += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 y 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">y 分量增量。</param>
        public static void AddLocalScaleY(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.y += deltaValue;
            transform.localScale = v;
        }

        /// <summary>
        /// 增加相对尺寸的 z 分量。
        /// </summary>
        /// <param name="transform"><see cref="UnityEngine.Transform" /> 对象。</param>
        /// <param name="deltaValue">z 分量增量。</param>
        public static void AddLocalScaleZ(this Transform transform, float deltaValue)
        {
            Vector3 v = transform.localScale;
            v.z += deltaValue;
            transform.localScale = v;
        }

        #endregion

        #region ... RectTransform
        public static void SetSizeDeltaX(this RectTransform rectTransform, float newXValue, float newYValue)
        {
            Vector2 v = rectTransform.sizeDelta;
            v.x = newXValue;
            v.x = newYValue;
            rectTransform.sizeDelta = v;
        }

        public static void SetSizeDeltaX(this RectTransform rectTransform, float newValue)
        {
            Vector2 v = rectTransform.sizeDelta;
            v.x = newValue;
            rectTransform.sizeDelta = v;
        }

        public static void SetSizeDeltaY(this RectTransform rectTransform, float newValue)
        {
            Vector2 v = rectTransform.sizeDelta;
            v.y = newValue;
            rectTransform.sizeDelta = v;
        }

        public static void AddSizeDeltaX(this RectTransform rectTransform, float deltaValue)
        {
            Vector2 v = rectTransform.sizeDelta;
            v.x += deltaValue;
            rectTransform.sizeDelta = v;
        }

        public static void AddSizeDeltaY(this RectTransform rectTransform, float deltaValue)
        {
            Vector2 v = rectTransform.sizeDelta;
            v.y += deltaValue;
            rectTransform.sizeDelta = v;
        }

        public static Vector3[] GetLocalPositions(this RectTransform[] rectTfs)
        {
            if (rectTfs == null || rectTfs.Length == 0)
                return null;
            Vector3[] vec3s = new Vector3[rectTfs.Length];
            for (int i = 0; i < rectTfs.Length; i++)
            {
                if (rectTfs[i])
                {
                    vec3s[i] = rectTfs[i].localPosition;
                }
            }
            return vec3s;
        }

        public static Vector3[] GetSizeDeltas(this RectTransform[] rectTfs)
        {
            if (rectTfs == null || rectTfs.Length == 0)
                return null;
            Vector3[] vec3s = new Vector3[rectTfs.Length];
            for (int i = 0; i < rectTfs.Length; i++)
            {
                if (rectTfs[i])
                {
                    vec3s[i] = rectTfs[i].sizeDelta;
                }
            }
            return vec3s;
        }

        #endregion

        #endregion

        #region ... GameObject

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <typeparam name="T">要获取或增加的组件。</typeparam>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>获取或增加的组件。</returns>
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }

            return component;
        }

        /// <summary>
        /// 获取或增加组件。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <param name="type">要获取或增加的组件类型。</param>
        /// <returns>获取或增加的组件。</returns>
        public static Component GetOrAddComponent(this GameObject gameObject, Type type)
        {
            Component component = gameObject.GetComponent(type);
            if (component == null)
            {
                component = gameObject.AddComponent(type);
            }

            return component;
        }

        /// <summary>
        /// 获取 GameObject 是否在场景中。
        /// </summary>
        /// <param name="gameObject">目标对象。</param>
        /// <returns>GameObject 是否在场景中。</returns>
        /// <remarks>若返回 true，表明此 GameObject 是一个场景中的实例对象；若返回 false，表明此 GameObject 是一个 Prefab。</remarks>
        public static bool InScene(this GameObject gameObject)
        {
            return gameObject.scene.name != null;
        }

        #endregion

        #region ... Vector 2/3

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector3" /> 的 (x, y, z) 转换为 <see cref="UnityEngine.Vector2" /> 的 (x, z)。
        /// </summary>
        /// <param name="vector3">要转换的 Vector3。</param>
        /// <returns>转换后的 Vector2。</returns>
        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 0, y)。
        /// </summary>
        /// <param name="vector2">要转换的 Vector2。</param>
        /// <returns>转换后的 Vector3。</returns>
        public static Vector3 ToVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0f, vector2.y);
        }

        /// <summary>
        /// 取 <see cref="UnityEngine.Vector2" /> 的 (x, y) 和给定参数 y 转换为 <see cref="UnityEngine.Vector3" /> 的 (x, 参数 y, y)。
        /// </summary>
        /// <param name="vector2">要转换的 Vector2。</param>
        /// <param name="y">Vector3 的 y 值。</param>
        /// <returns>转换后的 Vector3。</returns>
        public static Vector3 ToVector3(this Vector2 vector2, float y)
        {
            return new Vector3(vector2.x, y, vector2.y);
        }

        /// <summary>
        /// 根据 size尺寸 获取斜切 最长长度
        /// </summary>
        /// <param name="vector2"></param>
        /// <returns></returns>
        public static float GetSqrt(this Vector2 vector2)
        {
            float width = vector2.x;
            float height = vector2.y;
            if (width < 0) width = Mathf.Abs(width);
            if (height < 0) height = Mathf.Abs(height);
            float sqrt = Mathf.Sqrt((width * width) + (height * height));
            return sqrt;
        }

        #endregion

        #region ... 基础数据结构 

        #region ... string

        public static Vector2 StrToVec2(this string vec2, char separator = ',', string errMsg = "")
        {
            if (!string.IsNullOrEmpty(vec2))
            {
                string[] vec2s = vec2.Split(separator);
                if (vec2s.Length == 2)
                {
                    return new Vector2(float.Parse(vec2s[0]), float.Parse(vec2s[1]));
                }
            }

            Debug.LogErrorFormat("[UnityExtends].StrToVec2 fail, vec2  '{0}' , separator '{1}', errMsg '{2}'  ", vec2, separator, errMsg);
            return Vector2.zero;
        }

        public static Vector3 StrToVec3(this string vec3, char separator = ',', string errMsg = "")
        {
            if (!string.IsNullOrEmpty(vec3))
            {
                string[] vec3s = vec3.Split(separator);
                if (vec3s.Length == 3)
                {
                    return new Vector3(float.Parse(vec3s[0]), float.Parse(vec3s[1]), float.Parse(vec3s[2]));
                }
                else if (vec3s.Length == 2)
                {
                    return new Vector3(float.Parse(vec3s[0]), float.Parse(vec3s[1]), 0);
                }
            }
            Debug.LogErrorFormat("[UnityExtends].StrToVec3 fail, vec3  '{0}', separator '{1}', errMsg '{2}'.  ", vec3, separator, errMsg);
            return default(Vector3);
        }

        /// <summary>
        /// string 强转Color
        /// </summary>
        /// <param name="colorStr"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static Color ParseColor(this string colorStr, char separator = ',', string errMsg = "")
        {
            Color color = Color.white;
            if (!string.IsNullOrEmpty(colorStr))
            {
                string[] colors = colorStr.Split(separator);
                if (colors != null)
                {
                    float r = 1, g = 1, b = 1, a = 1;
                    if (colors.Length == 3)
                    {
                        r = (float)int.Parse(colors[0]) / 255;
                        g = (float)int.Parse(colors[1]) / 255;
                        b = (float)int.Parse(colors[2]) / 255;
                        color = new Color(r, g, b, a);
                    }
                    else if (colors.Length == 4)
                    {
                        r = (float)int.Parse(colors[0]) / 255;
                        g = (float)int.Parse(colors[1]) / 255;
                        b = (float)int.Parse(colors[2]) / 255;
                        a = (float)int.Parse(colors[3]) / 255;
                        color = new Color(r, g, b, a);
                    }
                    return color;
                }
            }
            Debug.LogErrorFormat("[UnityExtends].ParseColor fail, colorStr  '{0}', separator '{1}',errMsg '{2}'  ", colorStr, separator, errMsg);
            return color;
        }
        /// <summary>
        /// string 十六进制 转换颜色 自动判断#号不存在添加
        /// </summary>
        /// <param name="hexadecimalColorStr"></param>
        /// <returns></returns>
        public static Color ParseColorByHexadecimal(this string hexadecimalColorStr)
        {
            if (!hexadecimalColorStr.Contains("#"))
            {
                hexadecimalColorStr = "#" + hexadecimalColorStr;
            }
            Color color;
            if (ColorUtility.TryParseHtmlString(hexadecimalColorStr, out color))
            {
                return color;
            }
            Debug.LogErrorFormat("[UnityExtends].ParseColorByHexadecimal fail, color string  '{0}'   ", hexadecimalColorStr);
            return Color.white;
        }

        /// <summary>
        /// string 强转bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool ParseBool(this string str, string errMsg = "")
        {
            bool result;
            if (bool.TryParse(str, out result))
            {
                return result;
            }
            Debug.LogErrorFormat("[UnityExtension] BoolPause fail  ,str  '{0}' , errMsg '{1}'. ", str, errMsg);
            return false;
        }

        /// <summary>
        /// 强转 1=True 0=False
        /// </summary>
        /// <param name="str"></param>
        /// <returns>1=True 0=False</returns>
        public static bool ParseBoolIn1or0(this string str)
        {
            switch (str)
            {
                case "1":
                    return true;
                case "0":
                    return false;
                default:
                    Debug.LogErrorFormat("[UnityExtension] ParseBoolIn1or0 error " + str);
                    return false;
            }
        }

        #endregion

        #region ... string 2 array

        public static string[] StrToStrs(this string str, char separator = ',', string errMsg = "")
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] datas = str.Split(separator);
                return datas;
            }
            Debug.LogErrorFormat("[UnityExtends].StrToStrs fail, str  '{0}' separator '{1}' , errMsg '{2}'  ", str, separator, errMsg);
            return default(string[]);
        }

        public static int[] StrToInts(this string str, char separator = ',', string errMsg = "")
        {
            if (!string.IsNullOrEmpty(str))
            {
                string[] datas = str.Split(separator);
                return datas.StrsToInts();
            }
            Debug.LogErrorFormat("[UnityExtends].StrToStrs fail, str  '{0}' separator '{1}'  errMsg '{2}' ", str, separator, errMsg);
            return default(int[]);
        }

        #endregion

        #region ... strings 2 array 

        public static int[] StrsToInts(this string[] strs, string errMsg = "")
        {
            if (strs == null || strs.Length == 0)
            {
                Debug.LogErrorFormat("[UnityExtends].StrsToInts fail, count = 0  errMsg " + errMsg);
                return default(int[]);
            }
            int[] ints = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                if (!int.TryParse(strs[i], out ints[i]))
                {
                    Debug.LogErrorFormat("[UnityExtends].StrsToInts fail, data  '{0}'  ,index '{1}'. errMsg '{2}'  ", strs[i], i, errMsg);
                }
            }
            return ints;
        }

        public static Vector2[] StrsToVec2s(this string[] strs, char vec3Separator = ',', string errMsg = "")
        {
            if (strs == null || strs.Length == 0)
            {
                return default(Vector2[]);
            }
            Vector2[] vec2s = new Vector2[strs.Length];
            for (int i = 0; i < vec2s.Length; i++)
            {
                vec2s[i] = StrToVec2(strs[i], vec3Separator, errMsg + " >>> i:" + i);
            }
            return vec2s;
        }

        public static Vector3[] StrsToVec3s(this string[] strs, char vec3Separator = ',', string errMsg = "")
        {
            if (strs == null || strs.Length == 0)
            {
                return default(Vector3[]);
            }
            Vector3[] vec3s = new Vector3[strs.Length];
            for (int i = 0; i < vec3s.Length; i++)
            {
                vec3s[i] = StrToVec3(strs[i], vec3Separator, errMsg + " >>> i:" + i);
            }
            return vec3s;
        }

        #endregion

        #region ... int

        /// <summary>
        /// 转换下载速度
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string ToDownloadSpeedStr(this int length)
        {
            if (length < 1024)
            {
                return string.Format("{0} Bytes", length.ToString());
            }

            if (length < 1024 * 1024)
            {
                return string.Format("{0} KB", (length / 1024f).ToString("F2"));
            }

            if (length < 1024 * 1024 * 1024)
            {
                return string.Format("{0} MB", (length / 1024f / 1024f).ToString("F2"));
            }

            return string.Format("{0} GB", (length / 1024f / 1024f / 1024f).ToString("F2"));
        }

        public static int TryMinOrMax(this int num, int min, int max)
        {
            if (num > max)
            {
                Debug.LogErrorFormat("[TryMinOrMax] num '{0}' > max '{1}' ", num, max);
                return max;
            }
            else if (num < min)
            {
                Debug.LogErrorFormat("[TryMinOrMax] num '{0}' < min '{1}' ", num, min);
                return min;
            }
            else
            {
                return num;
            }
        }

        #endregion

        #endregion

        #region UI

        public static void SetText(this Text[] texts, string value)
        {
            if (texts != null && texts.Length > 0)
            {
                for (int i = 0; i < texts.Length; i++)
                {
                    if (texts[i])
                        texts[i].text = value;
                }
            }
        }

        public static void SetText(this List<Text> texts, string value)
        {
            if (texts != null && texts.Count > 0)
            {
                for (int i = 0; i < texts.Count; i++)
                {
                    if (texts[i])
                        texts[i].text = value;
                }
            }
        }

        public static void CopyText(this Text text, Text copyToText)
        {
            if (copyToText != null && text != null)
            {
                copyToText.text = text.text;
            }
        }

        /// <summary>
        /// 设置 value 值 根据文本比对获取索引
        /// </summary>
        /// <param name="dropdown"></param>
        /// <param name="text"></param>
        public static void SetValueByText(this Dropdown dropdown, string text)
        {
            int index = dropdown.options.FindIndex((p) => { return p.text == text; });
            dropdown.value = index == -1 ? 0 : index;
        }

        #endregion
    }
}

