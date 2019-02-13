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
    public class FileStream : Stream
    {
        private int readTimeout = -1;
        private int writeTimeout = 1000;
        internal const int DefaultBufferSize = 8192;
        private FileRandomAccessStream backend;
        private FileOptions fileOptions;
        private string name;

        public override bool CanRead { get { return this.backend.CanRead; } }
        public override bool CanSeek { get { return true; } }
        public override bool CanWrite { get { return this.backend.CanWrite; } }
        public virtual bool IsAsync { get { return (this.fileOptions & FileOptions.Asynchronous) > FileOptions.None; } }
        public override long Length { get { return (long)this.backend.Size; } }
        public override int ReadTimeout { get { return this.readTimeout; } set { this.readTimeout = value; } }
        public override int WriteTimeout { get { return this.writeTimeout; } set { this.writeTimeout = value; } }

        public string Name { get { return this.name; } }

        public override long Position
        {
            get { return (long)this.backend.Position; }
            set
            {
                try
                {
                    this.backend.Seek((ulong)value);
                }
                catch (Exception ex)
                {
                    throw FileStream.RethrowException(ex);
                }
            }
        }

        public FileStream(string file, FileMode mode)
            : this(file, mode, mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite, FileShare.Read, 8192, FileOptions.None)
        {
        }

        public FileStream(string file, FileMode mode, FileAccess access)
            : this(file, mode, access, FileShare.Read, 8192, FileOptions.None)
        {
        }

        public FileStream(string file, FileMode mode, FileAccess access, FileShare share)
            : this(file, mode, access, share, 8192, FileOptions.None)
        {
        }

        public FileStream(string file, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            : this(file, mode, access, share, bufferSize, FileOptions.None)
        {
        }

        public FileStream(string file, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            : this(file, mode, access, share, bufferSize, useAsync ? FileOptions.Asynchronous : FileOptions.None)
        {
        }

        public FileStream(string file, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
        {
            try
            {
                this.fileOptions = options;
                this.name = file;
                this.CheckAccess(mode, access);
                StorageFile storageFile;
                switch (mode)
                {
                    case FileMode.CreateNew:
                    case FileMode.Create:
                    case FileMode.OpenOrCreate:
                    case FileMode.Append:
                        storageFile = FileStream.CreateFile(file, mode, access);
                        break;
                    case FileMode.Open:
                    case FileMode.Truncate:
                        storageFile = FileStream.OpenFile(file, mode, access);
                        break;
                    default:
                        throw new ArgumentException("Unknown file mode");
                }
                IAsyncOperation<IRandomAccessStream> source = storageFile.OpenAsync(FileStream.GetAccessMode(access));
                WindowsRuntimeSystemExtensions.AsTask<IRandomAccessStream>(source).Wait();
                this.backend = (FileRandomAccessStream)source.GetResults();
                if (mode == FileMode.Truncate)
                {
                    this.backend.Size = 0UL;
                }
                else
                {
                    if (mode != FileMode.Append)
                        return;
                    this.backend.Seek(this.backend.Size);
                }
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        private void CheckAccess(FileMode mode, FileAccess access)
        {
            switch (mode)
            {
                case FileMode.CreateNew:
                    break;
                case FileMode.Create:
                    break;
                case FileMode.Open:
                    break;
                case FileMode.OpenOrCreate:
                    break;
                case FileMode.Truncate:
                    break;
                case FileMode.Append:
                    if (access == FileAccess.Write)
                        break;
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("FileStream.CheckAccess: Bad access mode for Append");
                        throw new IOException("Bad access mode for Append");
                    }
                default:
                    throw new ArgumentException("Unknown file mode");
            }
        }

        private static StorageFile OpenFile(string file, FileMode mode, FileAccess access)
        {
            return FileHelper.GetFileForPathOrURI(file);
        }

        private static StorageFile CreateFile(string file, FileMode mode, FileAccess access)
        {
            IAsyncOperation<StorageFile> fileAsync = FileHelper.GetFolderForPathOrURI(file).CreateFileAsync(Path.GetFileName(file), FileStream.GetCollisionOption(mode, access));
            WindowsRuntimeSystemExtensions.AsTask<StorageFile>(fileAsync).Wait();
            if (fileAsync.Status != AsyncStatus.Completed)
            {
                System.Diagnostics.Debug.WriteLine("FileStream.CheckAccess: Failed to create file " + file);
                throw new IOException("Failed to create file " + file);
            }
            else
                return fileAsync.GetResults();
        }

        public override void Flush()
        {
            try
            {
                WindowsRuntimeSystemExtensions.AsTask<bool>(this.backend.FlushAsync()).Wait();
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        public void Flush(bool flushToDisc)
        {
            Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            try
            {
                Windows.Storage.Streams.Buffer buffer1 = new Windows.Storage.Streams.Buffer((uint)count);
                WindowsRuntimeSystemExtensions.AsTask<IBuffer, uint>(this.backend.ReadAsync((IBuffer)buffer1, (uint)count, InputStreamOptions.ReadAhead)).Wait(this.readTimeout);
                int length = (int)buffer1.Length;
                DataReader dataReader = DataReader.FromBuffer((IBuffer)buffer1);
                bool flag = offset == 0 && buffer.Length == count && length == count;
                byte[] numArray = flag ? buffer : new byte[length];
                dataReader.ReadBytes(numArray);
                if (!flag)
                    Array.Copy((Array)numArray, 0, (Array)buffer, offset, numArray.Length);
                return length;
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            try
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        if (offset > (long)this.backend.Size)
                        {
                            offset = (long)this.backend.Size;
                            break;
                        }
                        else
                            break;
                    case SeekOrigin.Current:
                        if ((long)this.backend.Position + offset > (long)this.backend.Size)
                        {
                            offset = (long)this.backend.Position + offset;
                            break;
                        }
                        else
                            break;
                    case SeekOrigin.End:
                        if (offset >= 0L)
                        {
                            offset = (long)this.backend.Size;
                            break;
                        }
                        else
                        {
                            offset += (long)this.backend.Size;
                            break;
                        }
                }
                if (offset < 0L)
                    offset = 0L;
                this.backend.Seek((ulong)offset);
                return offset;
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        public override void SetLength(long value)
        {
            try
            {
                this.backend.Size = (ulong)value;
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                Windows.Storage.Streams.Buffer buffer1 = new Windows.Storage.Streams.Buffer((uint)count);
                byte[] numArray;
                if (offset == 0 && count == buffer.Length)
                {
                    numArray = buffer;
                }
                else
                {
                    numArray = new byte[count];
                    Array.Copy((Array)buffer, offset, (Array)numArray, 0, count);
                }
                DataWriter dataWriter = new DataWriter();
                dataWriter.WriteBytes(numArray);
                WindowsRuntimeSystemExtensions.AsTask<uint, uint>(this.backend.WriteAsync(dataWriter.DetachBuffer())).Wait(this.writeTimeout);
            }
            catch (Exception ex)
            {
                throw FileStream.RethrowException(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.backend.Dispose();
        }

        public void Close()
        {
            base.Dispose();
        }

        private static Exception RethrowException(Exception e)
        {
            System.Diagnostics.Debug.WriteLine("FileStream.RethrowException: " + e.Message + "\n" + e.StackTrace);
            if (e.GetType() == typeof(IOException))
                return e;
            else
                return (Exception)new IOException(e.Message, e);
        }

        private static CreationCollisionOption GetCollisionOption(FileMode mode, FileAccess access)
        {
            CreationCollisionOption creationCollisionOption = CreationCollisionOption.GenerateUniqueName;
            switch (mode)
            {
                case FileMode.CreateNew:
                    creationCollisionOption = CreationCollisionOption.FailIfExists;
                    break;
                case FileMode.Create:
                case FileMode.Truncate:
                    creationCollisionOption = CreationCollisionOption.ReplaceExisting;
                    break;
                case FileMode.Open:
                case FileMode.OpenOrCreate:
                case FileMode.Append:
                    creationCollisionOption = CreationCollisionOption.OpenIfExists;
                    break;
            }
            return creationCollisionOption;
        }

        private static FileAccessMode GetAccessMode(FileAccess access)
        {
            switch (access)
            {
                case FileAccess.Read:
                    return FileAccessMode.Read;
                default:
                    return FileAccessMode.ReadWrite;
            }
        }
    }
}

#endif