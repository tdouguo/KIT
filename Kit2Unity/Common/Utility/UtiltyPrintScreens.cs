// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kit
{
    public static partial class Utility
    {
        /// <summary>
        ///  
        /// </summary> 
        public static class PrintScreens
        {

            #region ... 截屏

            /// <summary>
            /// 截屏，指定位置、尺寸、类型
            /// </summary>
            /// <param name="rect">截图的rect信息,默认 new Rect(0, 0, Screen.width, Screen.height) </param>
            /// <param name="ua">截图完毕后执行的函数</param>
            /// <param name="dest">截图的偏移量，默认 default(Vector2)</param>
            /// <returns></returns>
            public static IEnumerator PrintScreenAc(UnityAction<Texture2D> ua, Rect rect, Vector2 dest)
            {
                Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
                yield return Utility.WaitFPSEnd;
                texture.ReadPixels(rect, (int)dest.x, (int)dest.y);
                texture.Apply();
                if (ua != null)
                    ua(texture);
            }

            /// <summary>
            /// 截屏，指定位置、尺寸、类型
            /// </summary>
            /// <param name="rect">截图的rect信息</param>
            /// <param name="dest">截图的偏移量</param>
            public static Texture2D PrintScreen(Rect rect, Vector2 dest = default(Vector2))
            {
                Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
                texture.ReadPixels(rect, (int)dest.x, (int)dest.y);
                texture.Apply();
                return texture;
            }

            #endregion

        }
    }

}
