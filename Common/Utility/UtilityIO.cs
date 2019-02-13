// ----------------------------------------------------------------------------------------------------
// Copyright © Guo jin ming. All rights reserved.
// Homepage: https://kylin.app/
// E-Mail: kevin@kylin.app
// ----------------------------------------------------------------------------------------------------

using System;
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
        private const string BackupFileSuffixName = ".bak";

        #region ... IO.File

        /// <summary> 在指定目录下创建文本文件</summary>
        public static void CreateTextFile(string filePath, string contents)
        {
            CheckDirectory(filePath);
            File.WriteAllText(filePath, contents);
        }

        /// <summary> 在指定目录下创建二进制文件</summary>
        public static void CreateBytesFile(string filePath, byte[] bytes)
        {
            CheckDirectory(filePath);
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>获取指定路径的文件名，不包含后缀</summary>
        public static string GetFileName(string path)
        {
            path = path.Replace("\\", "/");
            if (path.IndexOf('/') >= 0)
            {
                if (path.IndexOf('.') > 0)
                    return path.Substring(path.LastIndexOf('/') + 1, path.LastIndexOf('.'));
                else
                    return path.Substring(path.LastIndexOf('/') + 1);
            }
            else
            {
                if (path.IndexOf('.') > 0)
                    return path.Substring(0, path.LastIndexOf('.'));
                else
                    return path;
            }
        }

        /// <summary>
        /// 删除删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool TryDeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

        #endregion

        #region ... IO.Binary  Reader/Writer

        #region ... Reader

        /// <summary>
        /// 二进制读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="writerCallback"></param>
        /// <param name="encoding"></param>
        public static void BinaryReader(string path, Action<BinaryReader> readerCallback, Encoding encoding = null)
        {
            if (!File.Exists(path))
            {
                throw new IOException("binary reader  fail ,error message  'There is no path'. ");
            }
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader;
                if (encoding != null)
                {
                    binaryReader = new BinaryReader(fileStream, encoding);
                }
                else
                {
                    binaryReader = new BinaryReader(fileStream);
                }

                if (readerCallback != null)
                    readerCallback.Invoke(binaryReader);

                binaryReader.Close();
                fileStream.Close();

                fileStream.Dispose();

                fileStream = null;
                binaryReader = null;
            }
            catch (Exception exception)
            {
                throw new IOException(Utility.Text.Format("binary reader fail exeption path '{0}',error message '{1}'", path, exception.Message));
            }
        }

        /// <summary>
        /// 二进制读取
        /// </summary>
        /// <param name="path"></param> 
        /// <param name="encoding"></param>
        public static byte[] BinaryReaderBytes(string path, Encoding encoding = null)
        {
            if (!File.Exists(path))
            {
                throw new IOException("binary reader  fail ,error message  'There is no path'. ");
            }
            FileStream fileStream = null;
            byte[] bytes = null;
            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader;
                if (encoding != null)
                {
                    binaryReader = new BinaryReader(fileStream, encoding);
                }
                else
                {
                    binaryReader = new BinaryReader(fileStream);
                }

                int count = binaryReader.ReadInt32();
                bytes = binaryReader.ReadBytes(count);

                fileStream.Close();
                binaryReader.Close();

                fileStream.Dispose();

                binaryReader = null;
                fileStream = null;

                return bytes;
            }
            catch (Exception exception)
            {
                throw new IOException(Utility.Text.Format("binary reader fail exeption path '{0}',error message '{1}' , error StackTrace '{2}'.",
                    path, exception.Message, exception.StackTrace));
            }
        }

        /// <summary>
        /// 二进制读取
        /// </summary>
        /// <param name="path"></param>
        /// <param name="writerCallback"></param>
        /// <param name="encoding"></param>
        public static string BinaryReaderString(string path, Encoding encoding = null)
        {
            if (!File.Exists(path))
            {
                throw new IOException("binary reader  fail ,error message  'There is no path'. ");
            }
            FileStream fileStream = null;
            string text = string.Empty;
            try
            {
                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader;
                if (encoding != null)
                {
                    binaryReader = new BinaryReader(fileStream, encoding);
                }
                else
                {
                    binaryReader = new BinaryReader(fileStream);
                }

                text = binaryReader.ReadString();

                binaryReader.Close();
                fileStream.Close();

                fileStream.Dispose();

                fileStream = null;
                binaryReader = null;
                return text;
            }
            catch (Exception exception)
            {
                throw new IOException(Utility.Text.Format("binary reader fail exeption path '{0}',error message '{1}'", path, exception.Message));
            }
        }

        #endregion

        #region ... Writer

        /// <summary>
        /// 二进制写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="writerCallback"></param>
        /// <param name="encoding"></param>
        public static bool BinaryWriter(string path, Action<BinaryWriter> writerCallback, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new IOException(Utility.Text.Format("binary writer  fail exeption path '{0}' .", path));
            }
            string backupFile = null;

            if (File.Exists(path))
            {
                backupFile = path + BackupFileSuffixName;
                if (File.Exists(backupFile))
                {
                    File.Delete(backupFile);
                }
                File.Move(path, backupFile);
            }

            CheckDirectory(path);

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                BinaryWriter binaryWriter = null;
                if (encoding != null)
                {
                    binaryWriter = new BinaryWriter(fileStream, encoding);
                }
                else
                {
                    binaryWriter = new BinaryWriter(fileStream);
                }

                if (writerCallback != null)
                    writerCallback.Invoke(binaryWriter);

                binaryWriter.Flush();
                fileStream.Flush();

                binaryWriter.Close();
                fileStream.Close();

                fileStream.Dispose();

                binaryWriter = null;
                fileStream = null;

                if (!string.IsNullOrEmpty(backupFile))
                {
                    File.Delete(backupFile);
                }
                return true;
            }
            catch (Exception exception)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (!string.IsNullOrEmpty(backupFile))
                {
                    File.Move(backupFile, path);
                }
                throw new IOException(Utility.Text.Format("binary writer  fail exeption path '{0}',error message '{1}'",
                    path, exception.Message));
            }
        }

        /// <summary>
        /// 二进制写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="writerCallback"></param>
        /// <param name="encoding"></param>
        public static bool BinaryWriterBytes(string path, byte[] bytes, Encoding encoding = null)
        {
            if ((bytes == null || bytes.Length == 0))
            {
                throw new IOException(Utility.Text.Format("binary writer  fail exeption path '{0}', bytes '{1}' , bytes count '{2}'.",
                    path, bytes, bytes != null ? bytes.Length : 0));
            }
            return BinaryWriter(path, (bw) =>
             {
                 bw.Write(bytes.Length);
                 bw.Write(bytes);
             }, encoding);
        }

        /// <summary>
        /// 二进制写入
        /// </summary>
        /// <param name="path"></param>
        /// <param name="writerCallback"></param>
        /// <param name="encoding"></param>
        public static bool BinaryWriterString(string path, string text, Encoding encoding = null)
        {
            return BinaryWriter(path, (bw) =>
             {
                 bw.Write(text);
             }, encoding);
        }

        #region ... File

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="path"></param>
        /// <param name="encoding">默认为null 则UTF-8 </param>
        /// <returns></returns>
        public static string FileReadText(string path, Encoding encoding = null)
        {
            try
            {
                string text = string.Empty;
                if (File.Exists(path))
                {
                    text = File.ReadAllText(path, encoding != null ? encoding : Encoding.UTF8);
                }
                return text;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("FileReadText error , path : '{0}' ,  message: '{1}'  , StackTrace: '{2}'.",
                    path, e.Message, e.StackTrace);
                return string.Empty;
            }

        }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="encoding">默认为null 则UTF-8 </param>
        /// <returns></returns>
        public static bool FileWriteText(string path, string text, Encoding encoding = null)
        {
            string backupFile = null;

            try
            {
                // 如果当前存在 则备份
                if (File.Exists(path))
                {
                    backupFile = path + BackupFileSuffixName;
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                    File.Move(path, backupFile);
                }

                // 写入
                File.WriteAllText(path, text, encoding != null ? encoding : Encoding.UTF8);

                // 移除备份
                if (!string.IsNullOrEmpty(backupFile))
                {
                    File.Delete(backupFile);
                }

                return true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("FileWriteText error , path : '{0}' ,  message: '{1}'  , StackTrace: '{2}'.",
                    path, e.Message, e.StackTrace);

                // 移除错误的文件
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                // 存在备份 则恢复
                if (!string.IsNullOrEmpty(backupFile))
                {
                    File.Move(backupFile, path);
                }
                return false;
            }

        }

        #endregion

        #endregion

        #endregion

        #region ... IO.Folder

        /// <summary>
        /// 检查文件路径是否存储
        /// </summary>
        /// <param name="fileName">文件名称</param>
        public static void CheckDirectory(string fileName)
        {
            string newDir = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(newDir))
                Directory.CreateDirectory(newDir);
        }

        ///// <summary>
        ///// 删除文件夹。 [测试无法删除]
        ///// </summary>
        ///// <param name="directoryName">要处理的根文件夹名称。</param>
        ///// <returns>是否移除空文件夹成功。</returns>
        //public static bool DeleteFolder(string directoryName)
        //{
        //    if (string.IsNullOrEmpty(directoryName))
        //    {
        //        Debug.LogError("Directory name is invalid.");
        //    }

        //    try
        //    {
        //        Debug.LogFormat("DeleteFolder '{0}' .  ", directoryName);

        //        if (!Directory.Exists(directoryName))
        //        {
        //            Debug.LogErrorFormat("Directory not exists  '{0}'.", directoryName);
        //            return false;
        //        }
        //        // 不使用 SearchOption.AllDirectories，以便于在可能产生异常的环境下删除尽可能多的目录
        //        string[] subDirectoryNames = Directory.GetDirectories(directoryName, "*");
        //        int subDirectoryCount = subDirectoryNames.Length;
        //        foreach (string subDirectoryName in subDirectoryNames)
        //        {
        //            if (DeleteFolder(subDirectoryName))
        //            {
        //                subDirectoryCount--;
        //            }
        //        }

        //        if (subDirectoryCount > 0)
        //        {
        //            return false;
        //        }

        //        if (Directory.GetFiles(directoryName, "*").Length > 0)
        //        {
        //            return false;
        //        }

        //        Directory.Delete(directoryName);
        //        Debug.LogFormat("DeleteFolder ok '{0}' .  ", directoryName);
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogErrorFormat("DeleteFolder error , directoryName : '{0}' ,  message: '{1}'  , StackTrace: '{2}'.",
        //            directoryName, e.Message, e.StackTrace);
        //        return false;
        //    }
        //}

        /// <summary>
        /// 删除文件夹里面部分内容
        /// </summary>
        /// <param name="folderPath"></param>
        /// <param name="ignore">忽略数组 注意 路径 为\  </param>
        public static bool DeleteFolder(string folderPath, string[] ignore = null)
        {
            if (!Directory.Exists(folderPath))
            {
                return true;
            }
            try
            {
                string[] fileNames = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);
                if (fileNames != null && fileNames.Length > 0)
                {
                    bool isIgnore = false;
                    foreach (string fileName in fileNames)
                    {
                        isIgnore = false;
                        if ((ignore != null && ignore.Length > 0))
                        {
                            if (Array.FindIndex(ignore, (p) => { return fileName.Contains(p); }) != -1)
                            {
                                isIgnore = true;
                            }
                        }
                        if (!isIgnore)
                            File.Delete(fileName);
                    }
                    Utility.Path.RemoveEmptyDirectory(folderPath);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("DeleteFolder error , directoryName : '{0}' ,  message: '{1}'  , StackTrace: '{2}'.",
                    folderPath, e.Message, e.StackTrace);
                return false;
            }

        }

        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirectoryName"></param>
        /// <param name="targetDirectoryName"></param>
        /// <param name="isForce"></param>
        public static void CopyFolder(string sourceDirectoryName, string targetDirectoryName, bool isForce = true)
        {
            try
            {
                // TODO:  Utility.Path.GetCombinePath 基于GameFramework 
                //string[] fileNames = Directory.GetFiles(sourceDirectoryName, "*", SearchOption.AllDirectories);
                //foreach (string fileName in fileNames)
                //{
                //    string destFileName = Utility.Path.GetCombinePath(targetDirectoryName, fileName.Substring(sourceDirectoryName.Length));
                //    FileInfo destFileInfo = new FileInfo(destFileName);
                //    if (!destFileInfo.Directory.Exists)
                //    {
                //        destFileInfo.Directory.Create();
                //    }

                //    if (isForce && File.Exists(destFileName))
                //    {
                //        File.Delete(destFileName);
                //    }

                //    File.Copy(fileName, destFileName);
                //}
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("CopyFolder error , sourceDirectoryName : '{0}' , targetDirectoryName : '{1}' , message: '{2}'  , StackTrace: '{3}'.",
                     sourceDirectoryName, targetDirectoryName, e.Message, e.StackTrace);
            }

        }

        /// <summary>获取指定文件夹下文件信息</summary>
        public static List<FileInfo> GetFileInfoByFolder(string folderPath, SearchOption option, string searchPattern = "*")
        {
            List<FileInfo> fileInfos = new List<FileInfo>();
            DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
            if (dirInfo.Exists)
            {
                FileInfo[] fis = dirInfo.GetFiles(searchPattern, option);
                if (fis.Length > 0)
                {
                    for (int i = 0; i < fis.Length; i++)
                    {
                        if (!fis[i].Name.EndsWith(".DS_Store") && !fis[i].Name.EndsWith(".meta"))
                        {
                            fileInfos.Add(fis[i]);
                        }
                    }
                }
            }
            return fileInfos;
        }

        #endregion

    }
}
 