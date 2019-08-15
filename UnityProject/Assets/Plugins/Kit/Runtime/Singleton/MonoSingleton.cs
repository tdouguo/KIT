// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

namespace Kit.Runtime
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(T)) as T;
                    if (instance == null)
                    {
                        instance = new GameObject("_" + typeof(T).Name).AddComponent<T>();
                        DontDestroyOnLoad(instance);
                    }
                    if (instance == null)
                        Debug.LogErrorFormat("Failed to create instance of {0}.", typeof(T).FullName);
                }
                return instance;
            }
        }

        void OnApplicationQuit() { if (instance != null) instance = null; }

        public static T CreateInstance()
        {
            if (Instance != null) Instance.OnCreate();
            return Instance;
        }

        protected virtual void OnCreate()
        {

        }
    }
}