#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BestHTTP.PlatformSupport.IO
{
    [Flags]
    public enum FileAccess
    {
        Read = 1,
        Write = 2,
        ReadWrite = Write | Read,
    }

    [Flags]
    public enum FileAttributes
    {
        Archive = 32,
        Compressed = 2048,
        Device = 64,
        Directory = 16,
        Encrypted = 16384,
        Hidden = 2,
        Normal = 128,
        NotContentIndexed = 8192,
        Offline = 4096,
        ReadOnly = 1,
        ReparsePoint = 1024,
        SparseFile = 512,
        System = 4,
        Temporary = 256,
    }

    public enum FileMode
    {
        CreateNew = 1,
        Create = 2,
        Open = 3,
        OpenOrCreate = 4,
        Truncate = 5,
        Append = 6,
    }

    [Flags]
    public enum FileOptions
    {
        None = 0,
        Encrypted = 16384,
        DeleteOnClose = 67108864,
        SequentialScan = 134217728,
        RandomAccess = 268435456,
        Asynchronous = 1073741824,
        WriteThrough = -2147483648,
    }

    [Flags]
    public enum FileShare
    {
        None = 0,
        Read = 1,
        Write = 2,
        ReadWrite = Write | Read,
        Delete = 4,
        Inheritable = 16,
    }
}
#endif