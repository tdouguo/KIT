// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Kit
{
    /// <summary>
    /// Unity 工具函数
    /// </summary>
    public  static partial class UtilityMisc
    {
        public static string ConfigFolderPath = Application.persistentDataPath + "/config/";
        public static string AssetBundleFolderPath = Application.persistentDataPath + "/AssetBundle/" + GetCurrentPlatform() + "/";
        public static string SpriteFolderPath = Application.persistentDataPath + "/sprite/";
        public static string ArchiveFolderPath = Application.persistentDataPath + "/archive/";
        
        /// <summary>获取当前平台类型字符串</summary>
        public static string GetCurrentPlatform()
        {
#if UNITY_ANDROID
            return "Android";
#elif UNITY_IPHONE
		    return "iOS";
#else
            return "StandaloneWindows64";
#endif
        }

        #region ... Load

        public static IEnumerator LoadAssetByWWW(string url, LoadComplete loadComplete)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("LoadAssetByWWW url is null or empty. ");
                yield break;
            }
            using (WWW www = new WWW(url))
            {
                yield return www;
                try
                {
                    if (www.isDone && string.IsNullOrEmpty(www.error))
                    {
                        loadComplete(www);
                    }
                    else
                    {
                        Debug.LogErrorFormat("LoadAssetByWWW load '{0}'.error '{1}'. ", url, www.error);
                        loadComplete(null, www.error);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("LoadAssetByWWW '{0}' error '{1}', {2}. ", url, e.Message, e.StackTrace);
                }
                finally
                {
                    www.Dispose();
                }
            }
        }

        public static IEnumerator LoadAssetBytesByWWW(string url, Action<byte[]> callback)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("load fail：url is null or empty. ");
                yield break;
            }
            yield return LoadAssetByWWW(url, (_www, err) =>
            {
                if (callback != null)
                    callback(_www != null ? _www.bytes : null);
            });
        }

        #endregion

        #region ... 图片

        public static IEnumerator LoadImage(string url, Action<Sprite> callback)
        {
            yield return LoadAssetByWWW(url, (_www, err) =>
            {
                Sprite sprite = null;
                if (_www != null)
                {
                    sprite = TextureToSprite(_www.texture);
                }
                if (callback != null)
                    callback(sprite);
            });
        }

        public static IEnumerator LoadImage(string url, Action<Sprite, byte[]> callback)
        {
            yield return LoadAssetByWWW(url, (_www, err) =>
            {
                Sprite sprite = null;
                byte[] bytes = null;
                if (_www != null)
                {
                    sprite = TextureToSprite(_www.texture);
                    bytes = _www.bytes;
                }
                if (callback != null)
                    callback(sprite, bytes);
            });
        }

        /// <summary> 下载图片并保存到指定目录</summary>
        public static IEnumerator DownloadImage(string url, Action<Sprite> callback = null, string path = null)
        {
            if (string.IsNullOrEmpty(url))
                yield break;
            WWW www = new WWW(url);
            yield return www;
            Sprite sprite = null;
            if (string.IsNullOrEmpty(www.error))
            {
                try
                {
                    if (!string.IsNullOrEmpty(path))
                    {
                        string filePath = SpriteFolderPath + path;
                        if (!Directory.Exists(filePath.Substring(0, filePath.LastIndexOf('/'))))
                            Directory.CreateDirectory(filePath.Substring(0, filePath.LastIndexOf('/')));
                        File.WriteAllBytes(filePath, www.texture.EncodeToPNG());
                    }
                    sprite = TextureToSprite(www.texture);
                    if (callback != null)
                        callback(sprite);
                }
                catch (Exception e)
                {
                    Debug.LogError("下载失败：url: " + url + ", error: " + e.Message);
                    if (callback != null)
                        callback(null);
                }
            }
            else
            {
                Debug.LogError("下载失败：url: " + url + ", error: " + www.error);
                if (callback != null)
                    callback(null);
            }
        }

        /// <summary> Texture转Sprite</summary>
        public static Sprite TextureToSprite(Texture texture)
        {
            Sprite sprite = null;
            if (texture)
            {
                Texture2D t2d = (Texture2D)texture;
                sprite = Sprite.Create(t2d, new Rect(0, 0, t2d.width, t2d.height), Vector2.zero);
            }
            return sprite;
        }

        /// <summary> Texture旋转</summary>
        public static Texture2D RotateTexture(Texture2D texture, float eulerAngles)
        {
            int x;
            int y;
            int i;
            int j;
            float phi = eulerAngles / (180 / Mathf.PI);
            float sn = Mathf.Sin(phi);
            float cs = Mathf.Cos(phi);
            Color32[] arr = texture.GetPixels32();
            Color32[] arr2 = new Color32[arr.Length];
            int W = texture.width;
            int H = texture.height;
            int xc = W / 2;
            int yc = H / 2;

            for (j = 0; j < H; j++)
            {
                for (i = 0; i < W; i++)
                {
                    arr2[j * W + i] = new Color32(0, 0, 0, 0);

                    x = (int)(cs * (i - xc) + sn * (j - yc) + xc);
                    y = (int)(-sn * (i - xc) + cs * (j - yc) + yc);

                    if ((x > -1) && (x < W) && (y > -1) && (y < H))
                    {
                        arr2[j * W + i] = arr[y * W + x];
                    }
                }
            }

            Texture2D newImg = new Texture2D(W, H);
            newImg.SetPixels32(arr2);
            newImg.Apply();

            return newImg;
        }

        /// <summary> 双线性插值法缩放图片，等比缩放</summary>
        public static Texture2D ScaleTextureBilinear(Texture2D originalTexture, float scaleFactor)
        {
            Texture2D newTexture = new Texture2D(Mathf.CeilToInt(originalTexture.width * scaleFactor), Mathf.CeilToInt(originalTexture.height * scaleFactor));
            float scale = 1.0f / scaleFactor;
            int maxX = originalTexture.width - 1;
            int maxY = originalTexture.height - 1;
            for (int y = 0; y < newTexture.height; y++)
            {
                for (int x = 0; x < newTexture.width; x++)
                {
                    // Bilinear Interpolation  
                    float targetX = x * scale;
                    float targetY = y * scale;
                    int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                    int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                    int x2 = Mathf.Min(maxX, x1 + 1);
                    int y2 = Mathf.Min(maxY, y1 + 1);

                    float u = targetX - x1;
                    float v = targetY - y1;
                    float w1 = (1 - u) * (1 - v);
                    float w2 = u * (1 - v);
                    float w3 = (1 - u) * v;
                    float w4 = u * v;
                    Color color1 = originalTexture.GetPixel(x1, y1);
                    Color color2 = originalTexture.GetPixel(x2, y1);
                    Color color3 = originalTexture.GetPixel(x1, y2);
                    Color color4 = originalTexture.GetPixel(x2, y2);
                    Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
                        Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
                        Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
                        Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
                    );
                    newTexture.SetPixel(x, y, color);

                }
            }
            newTexture.Apply();
            return newTexture;
        }

        /// <summary>将图片缩放为指定尺寸</summary>
        public static Texture2D SizeTextureBilinear(Texture2D originalTexture, Vector2 size)
        {
            Texture2D newTexture = new Texture2D(Mathf.CeilToInt(size.x), Mathf.CeilToInt(size.y));
            float scaleX = originalTexture.width / size.x;
            float scaleY = originalTexture.height / size.y;
            int maxX = originalTexture.width - 1;
            int maxY = originalTexture.height - 1;
            for (int y = 0; y < newTexture.height; y++)
            {
                for (int x = 0; x < newTexture.width; x++)
                {
                    float targetX = x * scaleX;
                    float targetY = y * scaleY;
                    int x1 = Mathf.Min(maxX, Mathf.FloorToInt(targetX));
                    int y1 = Mathf.Min(maxY, Mathf.FloorToInt(targetY));
                    int x2 = Mathf.Min(maxX, x1 + 1);
                    int y2 = Mathf.Min(maxY, y1 + 1);

                    float u = targetX - x1;
                    float v = targetY - y1;
                    float w1 = (1 - u) * (1 - v);
                    float w2 = u * (1 - v);
                    float w3 = (1 - u) * v;
                    float w4 = u * v;
                    Color color1 = originalTexture.GetPixel(x1, y1);
                    Color color2 = originalTexture.GetPixel(x2, y1);
                    Color color3 = originalTexture.GetPixel(x1, y2);
                    Color color4 = originalTexture.GetPixel(x2, y2);
                    Color color = new Color(Mathf.Clamp01(color1.r * w1 + color2.r * w2 + color3.r * w3 + color4.r * w4),
                        Mathf.Clamp01(color1.g * w1 + color2.g * w2 + color3.g * w3 + color4.g * w4),
                        Mathf.Clamp01(color1.b * w1 + color2.b * w2 + color3.b * w3 + color4.b * w4),
                        Mathf.Clamp01(color1.a * w1 + color2.a * w2 + color3.a * w3 + color4.a * w4)
                    );
                    newTexture.SetPixel(x, y, color);

                }
            }
            newTexture.Apply();
            return newTexture;
        }

        /// <summary> 在指定物体上添加指定图片 </summary>
        public static Image AddImage(GameObject target, Sprite sprite)
        {
            target.SetActive(false);
            Image image = target.GetComponent<Image>();
            if (!image)
                image = target.AddComponent<Image>();
            image.sprite = sprite;
            image.SetNativeSize();
            target.SetActive(true);
            return image;
        }

        #endregion

        #region ... 声音

        public static IEnumerator LoadAudioClip(string url, Action<AudioClip> callback)
        {
            yield return LoadAssetByWWW(url, (_www, err) =>
            {
                AudioClip audioClip = null;
                if (_www != null)
                {
                    audioClip = _www.GetAudioClip();
                }
                if (callback != null)
                    callback(audioClip);
            });
        }

        #endregion

        #region ... UI

        /// <summary> 角度转向量 </summary>
        public static Vector2 AngleToVector2D(float angle)
        {
            float radian = Mathf.Deg2Rad * angle;
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized;
        }

        /// <summary> 得到鼠标相对Canvas中心的位置 </summary>
        public static Vector2 GetMouseCenterPosInCanvas()
        {
            Vector2 mousePosition = Input.mousePosition;

            Vector2 middlePos = new Vector2(Screen.width / 2, Screen.height / 2);

            Vector2 endPos = middlePos - mousePosition;//最终位置
            return endPos = new Vector2(-1 * endPos.x, -1 * endPos.y);
        }

        /// <summary> 获取当前Canvas的Rect值,通过屏幕坐标转化 </summary>
        public static Rect GetRectInCanvas(Canvas canvas, RectTransform rectTrans)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                             rectTrans.position, canvas.worldCamera, out pos))
            {
                var rect = new Rect(new Vector2(pos.x - rectTrans.pivot.x * rectTrans.rect.width, pos.y - rectTrans.pivot.y * rectTrans.rect.height), rectTrans.rect.size);
                return rect;
            }

            throw new System.Exception("Error! Get RectTransform rect in canvas fail.");
        }

        /// <summary>
        /// canvars 计算匹配宽还是匹配高
        /// </summary>
        /// <param name="standardWidth">初始宽度</param>
        /// <param name="standardHeight">初始高度</param>
        /// <returns></returns>
        public static float CalculateMatchWidthOrHeight(float standardWidth, float standardHeight)
        {
            float device_width = 0f;            //当前设备宽度        
            float device_height = 0f;           //当前设备高度        
            float adjustor = 0f;                //屏幕矫正比例  

            //获取设备宽高        
            device_width = Screen.width;
            device_height = Screen.height;

            //计算宽高比例        
            float standard_aspect = standardWidth / standardHeight;
            float device_aspect = device_width / device_height;

            //计算矫正比例        
            if (device_aspect < standard_aspect)
            {
                adjustor = standard_aspect / device_aspect;
            }
            return adjustor == 0 ? 1 : 0;
        }

        #endregion

        #region ... Input

        // TODO: 此处内容 有可能需要挂载MonoBehaviour
        public static bool OnHeld(int index = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    return Input.GetMouseButton(index);
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    return Input.touchCount > index && (Input.GetTouch(index).phase == TouchPhase.Moved || Input.GetTouch(index).phase == TouchPhase.Stationary);
            }
            return false;
        }

        public static bool OnPressed(int index = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    return Input.GetMouseButtonDown(index);
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    return Input.touchCount > index && Input.GetTouch(index).phase == TouchPhase.Began;
            }
            return false;
        }

        public static bool OnReleased(int index = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    return Input.GetMouseButtonUp(index);
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    return Input.touchCount > index && (Input.GetTouch(index).phase == TouchPhase.Ended || Input.GetTouch(index).phase == TouchPhase.Canceled);
            }
            return false;
        }

        public static Vector2 GetTouchPosition(int index = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    return Input.mousePosition;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    if (index < Input.touchCount)
                        return Input.GetTouch(index).position;
                    break;
            }

            return Vector2.zero;
        }

        public static Vector2 GetTouchDeltaPosition(int index = 0)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WebGLPlayer:
                    return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    if (index < Input.touchCount)
                        return Input.GetTouch(index).deltaPosition;
                    return Vector2.zero;
            }

            return Vector2.zero;
        }

        public static DeviceOrientation GetDeviceOrientation()
        {
            return Input.deviceOrientation;
        }

        #endregion

    }

    /// <summary>
    /// 加载完成
    /// </summary>
    public delegate void LoadComplete(WWW www, string errorMsg = null);

}
