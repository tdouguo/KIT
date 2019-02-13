// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using Ionic.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Kit
{
    public static partial  class Utility
    {
        /// <summary>
        /// Zip.(require: Libraries/3rd/DotNetZip)
        /// </summary>
        public static class DotNetZip
        {

            #region ... 压缩 zip 文件

            /// <summary>
            /// 压缩 zip 文件
            /// </summary>
            /// <param name="fileNames"></param>
            /// <param name="zipFileName"></param>
            public static void ZipFiles(string[] fileNames, string zipFileName)
            {
                if (null == fileNames) return;
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < fileNames.Length; i++)
                    {
                        zip.AddFile(fileNames[i]);
                    }
                    zip.Save(zipFileName);
                }
            }

            /// <summary>
            /// 压缩 zip 目录
            /// </summary>
            /// <param name="directoryNames"></param>
            /// <param name="zipFileName"></param>
            public static void ZipDirectorys(string[] directoryNames, string zipFileName)
            {
                if (null == directoryNames) return;
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < directoryNames.Length; i++)
                    {
                        zip.AddDirectory(directoryNames[i]);
                    }
                    zip.Save(zipFileName);
                }
            }

            /// <summary>
            /// 压缩 zip 单个目录
            /// </summary>
            /// <param name="fileName"></param>
            /// <param name="zipFileName"></param>
            public static void ZipFile(string fileName, string zipFileName)
            {
                ZipFiles(new string[] { fileName }, zipFileName);
            }

            /// <summary>
            /// 压缩 zip 单个目录
            /// </summary>
            /// <param name="directoryName"></param>
            /// <param name="zipFileName"></param>
            public static void ZipDirectory(string directoryName, string zipFileName)
            {
                ZipDirectorys(new string[] { directoryName }, zipFileName);
            }
             
            public static void EnZipFile(string zipFileName, string[] fileName, List<byte[]> list)
            {
                if (fileName == null || list == null || (fileName.Length != list.Count))
                {
                    Debug.LogError("zip file faile .");
                    return;
                }
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(zipFileName));
                int count = fileName.Length;
                using (ZipFile zip = new ZipFile())
                {
                    for (int i = 0; i < count; i++)
                    {
                        zip.AddEntry(fileName[i], list[i]);
                    }
                    zip.Save(zipFileName);
                }
            }

            public static void EnZipFile(string zipFileName, string fileName, byte[] bytes)
            {
                if (string.IsNullOrEmpty(zipFileName) || string.IsNullOrEmpty(fileName) || bytes == null || bytes.Length == 0)
                {
                    Debug.LogError("zip file faile .");
                    return;
                }
                int count = fileName.Length;
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddEntry(fileName, bytes);
                    zip.Save(zipFileName);
                }
            }

            #endregion

            #region ... 解压 zip 文件

            /// <summary>
            /// 解压 zip 文件
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="unZipPath"></param>
            /// <param name="progressCallback"></param>
            /// <param name="completeCallback"></param>
            public static void UnZipFile(string filePath, string unZipPath, EventHandler<UnZipProgressEventArgs> progressCallback = null,
                EventHandler completeCallback = null)
            {
                if (Directory.Exists(unZipPath))
                {
                    Directory.Delete(unZipPath, true);
                }
                //Log.Info("Un zip " + filePath + " " + unZipPath);
                using (ZipFile zip = Ionic.Zip.ZipFile.Read(filePath, new ReadOptions() { Encoding = Encoding.Default }))
                {
                    bool forOnceCompleteCallback = false;
                    zip.ExtractProgress += (sender, args) =>
                    {
                        if (0 == args.EntriesTotal)
                        {
                            return;
                        }

                        double progress = (double)args.EntriesExtracted / args.EntriesTotal;

                        if (null != progressCallback)
                        {
                            //正常解压进度。
                            if (1.0f >= progress && !forOnceCompleteCallback)
                            {
                                progressCallback.Invoke(sender, new UnZipProgressEventArgs(progress));
                            }
                        }
                        //正常解压进度的完成事件。
                        if (1.0f <= progress && !forOnceCompleteCallback)
                        {
                            if (null != completeCallback)
                            {
                                completeCallback.Invoke(sender, EventArgs.Empty);
                            }
                            forOnceCompleteCallback = true;
                        }
                    };
                    zip.ExtractAll(unZipPath);
                }
            }

            /// <summary>
            /// 压缩zip
            /// </summary>
            /// <param name="tempWorkPath">临时工作路径</param>
            /// <param name="fileName"></param>
            /// <param name="fileList"></param>
            /// <returns></returns>
            public static byte[] EnZipBytes(string tempWorkPath, string[] fileName, List<byte[]> fileList)
            {
                if (fileName == null || fileList == null || (fileName.Length != fileList.Count))
                {
                    Debug.LogError("not s3 pose files.");
                    return null;
                }

                string fname = UtilityTime.GetTimeStampStrByUtcNow() + ".zip";
                string zipFileName = Utility.Path.GetCombinePath(tempWorkPath, fname); // 获取工作目录
                EnZipFile(zipFileName, fileName, fileList);                                 // 把字节流打包成zip
                byte[] bytes = File.ReadAllBytes(zipFileName);                                   // 把文件读取到内存中
                File.Delete(zipFileName);                                                              // 删除本地硬盘上文件
                return bytes;
            }
             
            public static IEnumerator UnzipWithPath(string zipPath, string unZipDirPath, Action successAction, Action<string> failureAction)
            {
                ZipEntry zip = null;                                    // ZipEntry：文件条目 该目录下所有的文件列表(也就是所有文件的路径)  

                ZipInputStream zipInStream = null;                      // 输入的所有的文件流都是存储在这里面的  

                zipInStream = new ZipInputStream(File.OpenRead(zipPath));  // 读取文件流到zipInputStream  

                while ((zip = zipInStream.GetNextEntry()) != null)      // 循环读取Zip目录下的所有文件
                {
                    Debug.LogFormat("UnZip :  {0} , {1} , {2} , {3} .", zipInStream.Position, zipInStream.Length, zipPath, unZipDirPath);
                    UnzipFile(zip, zipInStream, unZipDirPath);
                    yield return WaitFPSEnd;
                }
                try
                {
                    zipInStream.Close();
                }
                catch (Exception e)
                {
                    Debug.Log("UnZip Failure " + e.Message);
                    if (failureAction != null)
                        failureAction.Invoke(e.Message);
                    yield break;
                }
                if (successAction != null)
                    successAction.Invoke();
                Debug.Log("UnZip Success：" + zipPath);
            }

            static void UnzipFile(ZipEntry zip, ZipInputStream zipInStream, string dirPath)
            {
                // Debug.LogFormat(" UnzipFile   zipName : '{0}' , dirPath : '{1}'  . ", zip.FileName, dirPath);
                try
                {
                    //文件名不为空  
                    if (!string.IsNullOrEmpty(zip.FileName))
                    {
                        string filePath = dirPath;
                        filePath += ("/" + zip.FileName);

                        if (IsDirectory(filePath))
                        {
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                        }
                        else
                        {
                            FileStream fs = null;
                            //当前文件夹下有该文件  删掉  重新创建  
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                            fs = File.Create(filePath);
                            int size = 2048;
                            byte[] data = new byte[2048];
                            //每次读取2MB  直到把这个内容读完  
                            while (true)
                            {
                                size = zipInStream.Read(data, 0, data.Length);
                                //小于0， 也就读完了当前的流  
                                if (size > 0)
                                {
                                    fs.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            fs.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat(" UnzipFile error,  zipName : '{0}' , dirPath : '{1}' ,   msg : '{2}' ,  StackTrace : '{3}' . ", zip.FileName, dirPath, e.Message, e.StackTrace);
                }
            }

            /// <summary>  
            /// 判断是否是目录文件  
            /// </summary>  
            /// <param name="path"></param>  
            /// <returns></returns>  
            static bool IsDirectory(string path)
            {
                if (path[path.Length - 1] == '/')
                {
                    return true;
                }
                return false;
            }

            #endregion
             
        }
    
    }
     
    public sealed class UnZipProgressEventArgs : EventArgs
    {
        public UnZipProgressEventArgs(double unZipProgress)
        {
            UnZipProgress = unZipProgress;
        }
        public double UnZipProgress { get; private set; }
    }
}
 