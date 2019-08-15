/*
--------------------------------------------------
| Copyright © 2008 Mr-Alan. All rights reserved. |
| Website: www.0x69h.com                         |
| Mail: mr.alan.china@gmail.com                  |
| QQ: 835988221                                  |
--------------------------------------------------
*/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Kit
{
    public static partial class UtilityUnity
    {
        /// <summary>
        /// 常用的WIN32 API辅助类。
        /// </summary>
        public static class UtilityWin32
        {
             
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
             
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
            public class OpenFileName
            {
                public int structSize = 0;
                public IntPtr dlgOwner = IntPtr.Zero;
                public IntPtr instance = IntPtr.Zero;
                public String filter = null;
                public String customFilter = null;
                public int maxCustFilter = 0;
                public int filterIndex = 0;
                public String file = null;
                public int maxFile = 0;
                public String fileTitle = null;
                public int maxFileTitle = 0;
                public String initialDir = null;
                public String title = null;
                public int flags = 0;
                public short fileOffset = 0;
                public short fileExtension = 0;
                public String defExt = null;
                public IntPtr custData = IntPtr.Zero;
                public IntPtr hook = IntPtr.Zero;
                public String templateName = null;
                public IntPtr reservedPtr = IntPtr.Zero;
                public int reservedInt = 0;
                public int flagsEx = 0;
            }

            [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
            public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

#endif
            /// <summary>
            /// 打开Win32文件对话框。(编辑器环境下切勿在运行对话框实例时切换编辑器状态，否则会报工作目录重定向异常并强制退出。)
            /// </summary>
            /// <param name="title">对话框标题。</param>
            /// <param name="filters">过滤文件扩展名。 用例 : new string[]{ "images","*.jpg;*.png" }</param>
            /// <param name="defExt">默认显示对应的文件扩展名文件。</param>
            /// <returns>文件名。</returns>
            public static string OpenFileDialogWin32(string title, string[] filters = null, string defExt = null)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                OpenFileName ofn = new OpenFileName();

                ofn.structSize = Marshal.SizeOf(ofn);

                var cfilter = string.Empty;
                if (filters != null && filters.Length > 0)
                {
                    for (int i = 0; i < filters.Length; i++)
                    {
                        if (i == (filters.Length - 1)) break;

                        var pam = string.Empty;
                        if (filters[i + 1].Contains(","))
                        {
                            var s = filters[i + 1].Split(',');
                            foreach (var item in s)
                            {
                                pam += String.Format("*.{0};", item);
                            }
                        }

                        cfilter += string.Format("{0}\0{1}\0", filters[i], pam);
                    }
                }


                ofn.filter = cfilter ?? "All Files\0*.*\0\0";

                ofn.file = new string(new char[256]);

                ofn.maxFile = ofn.file.Length;

                ofn.fileTitle = new string(new char[64]);

                ofn.maxFileTitle = ofn.fileTitle.Length;

                //ofn.initialDir = path;

                ofn.title = title;

                ofn.defExt = defExt;//显示文件的类型
                //注意 一下项目不一定要全选 但是0x00000008项不要缺少
                ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
                //OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

                if (GetOpenFileName(ofn))
                {
                    Debug.LogFormat("Selected file with full path: {0}", ofn.file);
                    return ofn.file;
                }

#else
                Debug.LogWarning( " Platform not supported." );
#endif
                return null;
            }
             
        } 
    }
}
