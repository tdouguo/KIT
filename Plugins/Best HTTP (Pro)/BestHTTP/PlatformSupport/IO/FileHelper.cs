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
    public static class FileHelper
    {
        internal const string LOCAL_FOLDER = "ms-appdata:///local/";
        internal const string ROAMING_FOLDER = "ms-appdata:///roaming/";
        internal const string TEMP_FOLDER = "ms-appdata:///temp/";
        internal const string STORE_FOLDER = "isostore:/";

        public static Stream OpenFileForReading(string uri)
        {
            return FileHelper.OpenFileForReading(FileHelper.GetFileForPathOrURI(uri));
        }

        public static Stream OpenFileForReading(System.Uri uri)
        {
            Task<StorageFile> task = WindowsRuntimeSystemExtensions.AsTask<StorageFile>(StorageFile.GetFileFromApplicationUriAsync(uri));
            task.Wait();
            if (task.Status != TaskStatus.RanToCompletion)
                throw new Exception("Filed to open file " + uri.ToString());
            else
                return FileHelper.OpenFileForReading(task.Result);
        }

        public static Stream OpenFileForWriting(string uri)
        {
            string fileName = Path.GetFileName(uri);
            Task<StorageFile> task1 = WindowsRuntimeSystemExtensions.AsTask<StorageFile>(FileHelper.GetFolderForPathOrURI(uri).CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting));
            task1.Wait();
            if (task1.Status != TaskStatus.RanToCompletion)
                throw new Exception("Failed to open the file");
            Task<IRandomAccessStream> task2 = WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(task1.Result.OpenAsync(FileAccessMode.ReadWrite));
            task2.Wait();
            if (task2.Status != TaskStatus.RanToCompletion)
                throw new Exception("Failed to open the file");
            else
                return WindowsRuntimeStreamExtensions.AsStreamForWrite((IOutputStream)task2.Result);
        }

        internal static StorageFolder GetFolderForURI(string uri)
        {
            uri = uri.ToLower();
            StorageFolder storageFolder1;
            if (uri.StartsWith("ms-appdata:///local/"))
            {
                storageFolder1 = ApplicationData.Current.LocalFolder;
                uri = uri.Replace("ms-appdata:///local/", "");
            }
            else if (uri.StartsWith("ms-appdata:///roaming/"))
            {
                storageFolder1 = ApplicationData.Current.RoamingFolder;
                uri = uri.Replace("ms-appdata:///roaming/", "");
            }
            else
            {
                if (!uri.StartsWith("ms-appdata:///temp/"))
                    throw new Exception("Unsupported URI: " + uri);
                storageFolder1 = ApplicationData.Current.TemporaryFolder;
                uri = uri.Replace("ms-appdata:///temp/", "");
            }
            string[] strArray = uri.Split(new char[1]
      {
        '/'
      });
            for (int index = 0; index < strArray.Length - 1; ++index)
            {
                Task<IReadOnlyList<StorageFolder>> task = WindowsRuntimeSystemExtensions.AsTask<IReadOnlyList<StorageFolder>>(storageFolder1.CreateFolderQuery().GetFoldersAsync());
                task.Wait();
                if (task.Status != TaskStatus.RanToCompletion)
                    throw new Exception("Failed to find folder: " + strArray[index]);
                IReadOnlyList<StorageFolder> result = task.Result;
                bool flag = false;
                foreach (StorageFolder storageFolder2 in (IEnumerable<StorageFolder>)result)
                {
                    if (storageFolder2.Name == strArray[index])
                    {
                        storageFolder1 = storageFolder2;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                    throw new Exception("Folder not found: " + strArray[index]);
            }
            return storageFolder1;
        }

        internal static StorageFolder GetFolderForPathOrURI(string path)
        {
            if (System.Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
                return FileHelper.GetFolderForURI(path);
            IAsyncOperation<StorageFolder> folderFromPathAsync = StorageFolder.GetFolderFromPathAsync(Path.GetDirectoryName(path));
            WindowsRuntimeSystemExtensions.AsTask<StorageFolder>(folderFromPathAsync).Wait();
            return folderFromPathAsync.GetResults();
        }

        internal static StorageFile GetFileForPathOrURI(string path)
        {
            IAsyncOperation<StorageFile> source = !System.Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute) ? StorageFile.GetFileFromPathAsync(path) : StorageFile.GetFileFromApplicationUriAsync(new System.Uri(path));
            WindowsRuntimeSystemExtensions.AsTask<StorageFile>(source).Wait();
            return source.GetResults();
        }

        internal static Stream OpenFileForReading(StorageFile file)
        {
            Task<IRandomAccessStream> task = WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(file.OpenAsync(FileAccessMode.Read));
            task.Wait();
            if (task.Status != TaskStatus.RanToCompletion)
                throw new Exception("Failed to open file!");
            else
                return WindowsRuntimeStreamExtensions.AsStreamForRead((IInputStream)task.Result);
        }

        internal static byte[] ReadEntireFile(StorageFile file)
        {
            Task<IBuffer> task = WindowsRuntimeSystemExtensions.AsTask<IBuffer>(FileIO.ReadBufferAsync((IStorageFile)file));
            task.Wait();
            if (task.Status != TaskStatus.RanToCompletion)
                throw new Exception("Failed to read file");
            IBuffer result = task.Result;
            DataReader dataReader = DataReader.FromBuffer(result);
            byte[] numArray = new byte[result.Length];
            dataReader.ReadBytes(numArray);
            return numArray;
        }
    }
}

#endif