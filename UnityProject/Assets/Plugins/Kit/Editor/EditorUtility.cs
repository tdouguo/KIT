// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Kit.Editor
{
    public class EditorUtility
    {
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <returns>
        /// The directory.
        /// </returns>
        /// <param name='path'>
        /// Path.
        /// </param>
        public static string CreateDirectory(string path)
        {
            Debug.LogWarning(Path.GetDirectoryName(path));
            DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(path));
            if (!dirInfo.Exists) dirInfo.Create();

            return path;
        }

        /// <summary>
        /// 获取资源路径
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetResourcePath(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return path.Remove(0, 7);
        }
    }
}