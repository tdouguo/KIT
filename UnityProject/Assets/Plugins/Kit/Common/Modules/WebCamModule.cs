// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Kit.Runtime
{
    /// <summary>
    /// 控制 摄像头
    /// </summary>
    public class WebCamModule
    {
        private string deviceName;

        private int cameraWidth;
        private int cameraHeight;
        private int cameraSize;
        private int cameraFPS;

        // 接收返回的图片数据  
        private WebCamTexture webCamTexture;
        private Texture2D texture2D;
        private Color32[] colorData;
        private byte[] byteData;

        public WebCamModule(int width, int height, int fps)
        {
            cameraWidth = width;
            cameraHeight = height;
            cameraFPS = fps;

            cameraSize = cameraWidth * cameraHeight * 3;
            colorData = new Color32[cameraWidth * cameraHeight];
            byteData = new byte[cameraSize];
        }

        /// <summary>
        /// 初始化当前模块
        /// </summary>
        /// <param name="rawImage"></param>
        /// <param name="succeedAction"></param>
        /// <param name="failueAction"></param>
        /// <returns></returns>
        public IEnumerator InitCamera(RawImage rawImage = null,
            UnityAction succeedAction = null, UnityAction failueAction = null)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
            if (Application.HasUserAuthorization(UserAuthorization.WebCam))
            {
                WebCamDevice[] devices = WebCamTexture.devices;
                deviceName = devices[0].name;

                for (int i = 0; i < devices.Length; i++)
                {
                    Debug.LogFormat("deviceName index: '{0}'  , name: '{1}'  .", i, devices[i].name);
                }

                webCamTexture = new WebCamTexture(deviceName, cameraWidth, cameraHeight, cameraFPS);

                texture2D = new Texture2D(cameraWidth, cameraHeight, TextureFormat.ARGB32, false);

                webCamTexture.Play();

                if (rawImage != null)
                {
                    rawImage.texture = texture2D;
                }

                if (succeedAction != null)
                {
                    succeedAction.Invoke();
                }

            }
            else
            {
                if (failueAction != null)
                {
                    failueAction.Invoke();
                }
            }
        }

        public void PlayCamera()
        {
            if (webCamTexture != null)
                webCamTexture.Play();
        }

        public void StopCamera()
        {
            if (webCamTexture != null)
                webCamTexture.Stop();
        }

        public byte[] GetByteData()
        {
            if (webCamTexture == null || !webCamTexture.isPlaying || !webCamTexture.didUpdateThisFrame)
                return null;

            //水平翻转
            Color32[] data = webCamTexture.GetPixels32();

            for (int h = 0; h < cameraHeight; ++h)
            {
                for (int w = 0; w < cameraWidth; ++w)
                {
                    int idx = h * cameraWidth + w;
                    int dstidx = h * cameraWidth + (cameraWidth - 1 - w);
                    colorData[dstidx] = data[idx];

                    int ii = dstidx * 3 + 2;
                    if (ii > byteData.Length || ii < 0)
                        ii = 0;

                    byteData[dstidx * 3 + 0] = colorData[dstidx].b;
                    byteData[dstidx * 3 + 1] = colorData[dstidx].g;
                    byteData[dstidx * 3 + 2] = colorData[dstidx].r;
                }
            }

            texture2D.SetPixels32(colorData);
            texture2D.Apply(false);

            return byteData;
        }

    }
}
