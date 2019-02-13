#if NETFX_CORE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Foundation;

namespace BestHTTP.PlatformSupport.IO
{
    public static class Directory
    {
        private static StorageFolder GetDirectoryForPath(string path)
        {
            IAsyncOperation<StorageFolder> folderFromPathAsync = StorageFolder.GetFolderFromPathAsync(path);
            WindowsRuntimeSystemExtensions.AsTask<StorageFolder>(folderFromPathAsync).Wait();
            return folderFromPathAsync.GetResults();
        }

        public static DirectoryInfo CreateDirectory(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("Path is empty");
            StorageFolder folder = (StorageFolder)null;
            path = path.Replace('/', '\\');
            string path1 = path;
            Stack<string> stack = new Stack<string>();
            do
            {
                try
                {
                    folder = Directory.GetDirectoryForPath(path1);
                    break;
                }
                catch
                {
                    int length = path1.LastIndexOf('\\');
                    if (length < 0)
                    {
                        path1 = (string)null;
                    }
                    else
                    {
                        stack.Push(path1.Substring(length + 1));
                        path1 = path1.Substring(0, length);
                    }
                }
            }
            while (path1 != null);
            if (path1 == null)
            {
                System.Diagnostics.Debug.WriteLine("Directory.CreateDirectory: Could not find any part of the path: " + path);
                throw new IOException("Could not find any part of the path: " + path);
            }
            try
            {
                while (stack.Count > 0)
                {
                    string desiredName = stack.Pop();
                    if (string.IsNullOrWhiteSpace(desiredName) && stack.Count > 0)
                        throw new ArgumentNullException("Empty directory name in the path");
                    IAsyncOperation<StorageFolder> folderAsync = folder.CreateFolderAsync(desiredName);
                    WindowsRuntimeSystemExtensions.AsTask<StorageFolder>(folderAsync).Wait();
                    folder = folderAsync.GetResults();
                }
                return new DirectoryInfo(path, folder);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.CreateDirectory: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.CreateDirectory: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static void Delete(string path)
        {
            Directory.Delete(path, false);
        }

        public static void Delete(string path, bool recursive)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty");
            try
            {
                StorageFolder directoryForPath = Directory.GetDirectoryForPath(path);
                if (!recursive)
                {
                    IAsyncOperation<IReadOnlyList<StorageFolder>> foldersAsync = directoryForPath.GetFoldersAsync();
                    WindowsRuntimeSystemExtensions.AsTask<IReadOnlyList<StorageFolder>>(foldersAsync).Wait();
                    if (Enumerable.Count<StorageFolder>((IEnumerable<StorageFolder>)foldersAsync.GetResults()) > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Directory.Delete: Directory not empty");
                        throw new IOException("Directory not empty");
                    }
                    IAsyncOperation<IReadOnlyList<StorageFile>> filesAsync = directoryForPath.GetFilesAsync();
                    WindowsRuntimeSystemExtensions.AsTask<IReadOnlyList<StorageFile>>(filesAsync).Wait();
                    if (Enumerable.Count<StorageFile>((IEnumerable<StorageFile>)filesAsync.GetResults()) > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Directory.Delete: Directory not empty");
                        throw new IOException("Directory not empty");
                    }
                }
                WindowsRuntimeSystemExtensions.AsTask(directoryForPath.DeleteAsync()).Wait();
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.Delete: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.Delete: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static IEnumerable<string> EnumerateDirectories(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty");
            try
            {
                IAsyncOperation<IReadOnlyList<StorageFolder>> foldersAsync = Directory.GetDirectoryForPath(path).GetFoldersAsync();
                WindowsRuntimeSystemExtensions.AsTask<IReadOnlyList<StorageFolder>>(foldersAsync).Wait();
                IReadOnlyList<StorageFolder> results = foldersAsync.GetResults();
                List<string> list = new List<string>(Enumerable.Count<StorageFolder>((IEnumerable<StorageFolder>)results));
                foreach (StorageFolder storageFolder in (IEnumerable<StorageFolder>)results)
                    list.Add(storageFolder.Path);
                return (IEnumerable<string>)list;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateDirectories: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateDirectories: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static IEnumerable<string> EnumerateFiles(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty");
            try
            {
                IAsyncOperation<IReadOnlyList<StorageFile>> filesAsync = Directory.GetDirectoryForPath(path).GetFilesAsync();
                WindowsRuntimeSystemExtensions.AsTask<IReadOnlyList<StorageFile>>(filesAsync).Wait();
                IReadOnlyList<StorageFile> results = filesAsync.GetResults();
                List<string> list = new List<string>(Enumerable.Count<StorageFile>((IEnumerable<StorageFile>)results));
                foreach (StorageFile storageFile in (IEnumerable<StorageFile>)results)
                    list.Add(storageFile.Path);
                return (IEnumerable<string>)list;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateFiles: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateFiles: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty");
            try
            {
                List<string> list = (List<string>)Directory.EnumerateDirectories(path);
                list.AddRange(Directory.EnumerateFiles(path));
                return (IEnumerable<string>)list;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateFileSystemEntries: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.EnumerateFileSystemEntries: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static bool Exists(string path)
        {
            try
            {
                return Directory.GetDirectoryForPath(path) != null;
            }
            catch
            {
                return false;
            }
        }

        private static DateTimeOffset GetFolderCreationTime(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path is empty");
            try
            {
                return Directory.GetDirectoryForPath(path).DateCreated;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.GetFolderCreationTime: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Directory.GetFolderCreationTime: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public static System.DateTime GetCreationTime(string path)
        {
            return Directory.GetFolderCreationTime(path).DateTime;
        }

        public static System.DateTime GetCreationTimeUtc(string path)
        {
            return Directory.GetFolderCreationTime(path).ToUniversalTime().DateTime;
        }

        public static string[] GetDirectories(string path)
        {
            return Enumerable.ToArray<string>(Directory.EnumerateDirectories(path));
        }

        public static string[] GetFiles(string path)
        {
            return Enumerable.ToArray<string>(Directory.EnumerateFiles(path));
        }

        public static string[] GetFileSystemEntries(string path)
        {
            return Enumerable.ToArray<string>(Directory.EnumerateFileSystemEntries(path));
        }
    }
}
#endif