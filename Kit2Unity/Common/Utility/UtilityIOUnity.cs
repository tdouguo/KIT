// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Kit
{
    /// <summary>
    ///  
    /// </summary>
    public static partial class UtilityIO
    { 
        #region ... IO

        /// <summary> 复制文件夹到指定目录</summary>
        public static void CopyDirectory(string sourceDirectoryPath, string targetDirectoryPath, string searchPattern = "*.*", bool isDeleteExist = false)
        {
            string[] files = Directory.GetFiles(sourceDirectoryPath, searchPattern, SearchOption.AllDirectories);
            string file, newPath, newDir;
            for (int i = 0; i < files.Length; i++)
            {
                file = files[i];
                file = file.Replace("\\", "/");
                if (!file.EndsWith(".meta") && !file.EndsWith(".DS_Store"))
                {
                    newPath = file.Replace(sourceDirectoryPath, targetDirectoryPath);
                    newDir = System.IO.Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(newDir))
                        Directory.CreateDirectory(newDir);
                    if (File.Exists(newPath))
                        if (isDeleteExist)
                            File.Delete(newPath);
                        else
                            continue;
                    if (Application.platform == RuntimePlatform.Android)
                        AndroidCopyFile(file, newPath);
                    else
                        File.Copy(file, newPath);
                }
            }
        }

        private static IEnumerator AndroidCopyFile(string sourceFilePath, string targetFilePath)
        {
            WWW www = new WWW("file://" + sourceFilePath);
            yield return www;
            File.WriteAllBytes(targetFilePath, UnicodeEncoding.UTF8.GetBytes(www.text));
        }
         
        #endregion
    }
}
