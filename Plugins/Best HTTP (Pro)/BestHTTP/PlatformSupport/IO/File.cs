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
    public static class File
    {
        public static void AppendAllLines(string path, IEnumerable<string> contents)
        {
            File.AppendAllLines(path, contents, (Encoding)null);
        }

        public static void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            try
            {
                IEnumerator<string> enumerator = contents.GetEnumerator();
                if (!enumerator.MoveNext())
                    return;
                using (StreamWriter streamWriter = File.AppendText(path, encoding))
                {
                    string current = enumerator.Current;
                    while (enumerator.MoveNext())
                    {
                        streamWriter.WriteLine(current);
                        current = enumerator.Current;
                    }
                    streamWriter.Write(current);
                }
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents, (Encoding)null);
        }

        public static void AppendAllText(string path, string contents, Encoding encoding)
        {
            try
            {
                using (StreamWriter streamWriter = File.AppendText(path, encoding))
                    streamWriter.Write(contents);
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static StreamWriter AppendText(string path, Encoding encoding)
        {
            try
            {
                IAsyncOperation<StorageFile> fileAsync = FileHelper.GetFolderForPathOrURI(path).CreateFileAsync(Path.GetFileName(path), CreationCollisionOption.OpenIfExists);
                WindowsRuntimeSystemExtensions.AsTask<StorageFile>(fileAsync).Wait();
                IAsyncOperation<IRandomAccessStream> source = fileAsync.GetResults().OpenAsync(FileAccessMode.ReadWrite);
                WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(source).Wait();
                FileRandomAccessStream randomAccessStream = (FileRandomAccessStream)source.GetResults();
                randomAccessStream.Seek(randomAccessStream.Size);
                return encoding == null ? new StreamWriter(WindowsRuntimeStreamExtensions.AsStream((IRandomAccessStream)randomAccessStream)) : new StreamWriter(WindowsRuntimeStreamExtensions.AsStream((IRandomAccessStream)randomAccessStream), encoding);
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        private static Exception GetRethrowException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("File.GetRethrowException: " + ex.Message + "\n" + ex.StackTrace);
            if (ex.GetType() == typeof(IOException))
                return ex;
            else
                return (Exception)new IOException(ex.Message, ex);
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName, false);
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                StorageFile fileForPathOrUri = FileHelper.GetFileForPathOrURI(sourceFileName);
                if (overwrite)
                {
                    StorageFile storageFile = (StorageFile)null;
                    try
                    {
                        storageFile = FileHelper.GetFileForPathOrURI(destFileName);
                    }
                    catch
                    {
                    }
                    if (storageFile != null)
                    {
                        WindowsRuntimeSystemExtensions.AsTask(fileForPathOrUri.CopyAndReplaceAsync((IStorageFile)storageFile)).Wait();
                        return;
                    }
                }
                WindowsRuntimeSystemExtensions.AsTask<StorageFile>(fileForPathOrUri.CopyAsync((IStorageFolder)FileHelper.GetFolderForPathOrURI(destFileName), Path.GetFileName(destFileName))).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static FileStream Create(string path)
        {
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
        }

        public static FileStream Create(string path, int bufferSize)
        {
            return new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None, bufferSize);
        }

        public static StreamWriter CreateText(string path)
        {
            return new StreamWriter((Stream)File.Create(path));
        }

        public static void Delete(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (path.Trim() == "")
                throw new ArgumentException();
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(FileHelper.GetFileForPathOrURI(path).DeleteAsync()).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static bool Exists(string path)
        {
            try
            {
                return FileHelper.GetFileForPathOrURI(path) != null;
            }
            catch
            {
                return false;
            }
        }

        public static FileAttributes GetAttributes(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                return File.WinAttributesToSysAttributes(FileHelper.GetFileForPathOrURI(path).Attributes);
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(ex.Message, ex);
            }
        }

        internal static FileAttributes WinAttributesToSysAttributes(Windows.Storage.FileAttributes atts)
        {
            FileAttributes fileAttributes = (FileAttributes)0;
            if ((Windows.Storage.FileAttributes.ReadOnly & atts) != Windows.Storage.FileAttributes.Normal)
                fileAttributes |= FileAttributes.ReadOnly;
            if ((Windows.Storage.FileAttributes.Directory & atts) != Windows.Storage.FileAttributes.Normal)
                fileAttributes |= FileAttributes.Directory;
            if ((Windows.Storage.FileAttributes.Archive & atts) != Windows.Storage.FileAttributes.Normal)
                fileAttributes |= FileAttributes.Archive;
            if ((Windows.Storage.FileAttributes.Temporary & atts) != Windows.Storage.FileAttributes.Normal)
                fileAttributes |= FileAttributes.Temporary;
            if (fileAttributes == (FileAttributes)0)
                fileAttributes = FileAttributes.Normal;
            return fileAttributes;
        }

        public static System.DateTime GetCreationTime(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                return FileHelper.GetFileForPathOrURI(path).DateCreated.DateTime;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(ex.Message, ex);
            }
        }

        public static System.DateTime GetCreationTimeUtc(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                return FileHelper.GetFileForPathOrURI(path).DateCreated.ToUniversalTime().DateTime;
            }
            catch (Exception ex)
            {
                throw new FileNotFoundException(ex.Message, ex);
            }
        }

        public static void Move(string sourceFileName, string destFileName)
        {
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(FileHelper.GetFileForPathOrURI(sourceFileName).MoveAsync((IStorageFolder)FileHelper.GetFolderForPathOrURI(destFileName), Path.GetFileName(destFileName))).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static FileStream Open(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }

        public static FileStream Open(string path, FileMode mode, FileAccess access)
        {
            return new FileStream(path, mode, access);
        }

        public static FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(path, mode, access, share);
        }

        public static FileStream OpenRead(string path)
        {
            return File.Open(path, FileMode.Open, FileAccess.Read);
        }

        public static StreamReader OpenText(string path)
        {
            return new StreamReader((Stream)File.OpenRead(path));
        }

        public static FileStream OpenWrite(string path)
        {
            return File.Open(path, FileMode.Create, FileAccess.Write);
        }

        public static byte[] ReadAllBytes(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                return FileHelper.ReadEntireFile(FileHelper.GetFileForPathOrURI(path));
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static string[] ReadAllLines(string path)
        {
            return Enumerable.ToArray<string>(File.ReadLines(path));
        }

        public static string[] ReadAllLines(string path, Encoding encoding)
        {
            return File.ReadAllLines(path);
        }

        public static string ReadAllText(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                IAsyncOperation<IRandomAccessStream> source = FileHelper.GetFileForPathOrURI(path).OpenAsync(FileAccessMode.Read);
                WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(source).Wait();
                using (FileRandomAccessStream randomAccessStream = (FileRandomAccessStream)source.GetResults())
                    return new StreamReader(WindowsRuntimeStreamExtensions.AsStreamForRead((IInputStream)randomAccessStream)).ReadToEnd();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path);
        }

        public static IEnumerable<string> ReadLines(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                IAsyncOperation<IRandomAccessStream> source = FileHelper.GetFileForPathOrURI(path).OpenAsync(FileAccessMode.Read);
                WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(source).Wait();
                using (FileRandomAccessStream randomAccessStream = (FileRandomAccessStream)source.GetResults())
                {
                    StreamReader streamReader = new StreamReader(WindowsRuntimeStreamExtensions.AsStreamForRead((IInputStream)randomAccessStream));
                    List<string> list = new List<string>();
                    while (true)
                    {
                        string str = streamReader.ReadLine();
                        if (str != null)
                            list.Add(str);
                        else
                            break;
                    }
                    return (IEnumerable<string>)list;
                }
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return File.ReadLines(path);
        }

        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            if (destinationFileName == null)
                throw new ArgumentNullException();
            if (!string.IsNullOrWhiteSpace(sourceFileName) && !string.IsNullOrWhiteSpace(destinationFileName))
            {
                if (!string.IsNullOrWhiteSpace(destinationBackupFileName))
                {
                    try
                    {
                        StorageFile fileForPathOrUri1 = FileHelper.GetFileForPathOrURI(sourceFileName);
                        string fileName1 = Path.GetFileName(destinationFileName);
                        StorageFile fileForPathOrUri2;
                        if (fileName1.ToLower() == destinationFileName.ToLower())
                        {
                            if (Path.GetFileName(sourceFileName).ToLower() == fileName1.ToLower())
                            {
                                System.Diagnostics.Debug.WriteLine("File.Replace: Source and destination is the same file");
                                throw new IOException("Source and destination is the same file");
                            }
                            fileForPathOrUri2 = FileHelper.GetFileForPathOrURI(Path.Combine(Path.GetDirectoryName(fileForPathOrUri1.Path), fileName1));
                        }
                        else
                        {
                            fileForPathOrUri2 = FileHelper.GetFileForPathOrURI(destinationFileName);
                            if (fileForPathOrUri1.Equals((object)fileForPathOrUri2))
                            {
                                System.Diagnostics.Debug.WriteLine("File.Replace: Source and destination is the same file");
                                throw new IOException("Source and destination is the same file");
                            }
                        }
                        string fileName2 = Path.GetFileName(destinationBackupFileName);
                        if (fileName2.ToLower() == destinationBackupFileName.ToLower())
                        {
                            WindowsRuntimeSystemExtensions.AsTask(fileForPathOrUri2.RenameAsync(destinationBackupFileName)).Wait();
                        }
                        else
                        {
                            StorageFolder folderForPathOrUri = FileHelper.GetFolderForPathOrURI(destinationBackupFileName);
                            WindowsRuntimeSystemExtensions.AsTask(fileForPathOrUri2.MoveAsync((IStorageFolder)folderForPathOrUri, fileName2)).Wait();
                        }
                        if (fileName1.ToLower() == destinationFileName.ToLower())
                        {
                            WindowsRuntimeSystemExtensions.AsTask(fileForPathOrUri1.RenameAsync(destinationFileName)).Wait();
                            return;
                        }
                        else
                        {
                            StorageFolder folderForPathOrUri = FileHelper.GetFolderForPathOrURI(destinationFileName);
                            WindowsRuntimeSystemExtensions.AsTask(fileForPathOrUri1.MoveAsync((IStorageFolder)folderForPathOrUri, fileName1)).Wait();
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw File.GetRethrowException(ex);
                    }
                }
            }
            throw new ArgumentException();
        }

        public static void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
        }

        private static StorageFile CreateOrReplaceFile(string path)
        {
            IAsyncOperation<StorageFile> fileAsync = FileHelper.GetFolderForPathOrURI(path).CreateFileAsync(Path.GetFileName(path), CreationCollisionOption.ReplaceExisting);
            WindowsRuntimeSystemExtensions.AsTask<StorageFile>(fileAsync).Wait();
            return fileAsync.GetResults();
        }

        public static void WriteAllBytes(string path, byte[] bytes)
        {
            if (path == null || bytes == null || bytes.Length == 0)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(FileIO.WriteBytesAsync((IStorageFile)File.CreateOrReplaceFile(path), bytes)).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents)
        {
            if (path == null || contents == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(FileIO.WriteLinesAsync((IStorageFile)File.CreateOrReplaceFile(path), contents)).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static void WriteAllLines(string path, string[] contents)
        {
            IEnumerable<string> contents1 = (IEnumerable<string>)contents;
            File.WriteAllLines(path, contents1);
        }

        public static void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            File.WriteAllLines(path, contents);
        }

        public static void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            File.WriteAllLines(path, contents);
        }

        public static void WriteAllText(string path, string contents)
        {
            if (path == null || string.IsNullOrEmpty(contents))
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(FileIO.WriteTextAsync((IStorageFile)File.CreateOrReplaceFile(path), contents)).Wait();
            }
            catch (Exception ex)
            {
                throw File.GetRethrowException(ex);
            }
        }

        public static void WriteAllText(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents);
        }
    }
}
#endif