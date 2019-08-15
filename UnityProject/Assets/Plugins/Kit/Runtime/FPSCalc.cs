// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace Kit.Runtime
{
    /// <summary>
    /// FPS calculate.
    /// </summary>
    public class FPSCalc : MonoSingleton<FPSCalc>
    {
        public bool ShowFPS = false;

        public float updateInterval = 0.1f;
        private float lastInterval;
        private int frames = 0;
        private float fps;


        public static void SetVisible(bool visible)
        {
            if (Instance.ShowFPS != visible)
            {
                Instance.ShowFPS = visible;
            }
        }

        void Start()
        {
            lastInterval = Time.realtimeSinceStartup;
            frames = 0;
        }

        void OnGUI()
        {
            if (ShowFPS)
            {
                if (fps > 30f)
                    GUI.color = Color.green;
                else if (fps > 15f)
                    GUI.color = Color.yellow;
                else
                    GUI.color = Color.red;

                GUILayout.Label(fps.ToString("f0"));
            }
        }

        void Update()
        {
            if (ShowFPS)
            {
                ++frames;
                float timeNow = Time.realtimeSinceStartup;
                if (timeNow > lastInterval + updateInterval)
                {
                    fps = (float)frames / (timeNow - lastInterval);
                    frames = 0;
                    lastInterval = timeNow;
                }
            }
        }
    }
}
