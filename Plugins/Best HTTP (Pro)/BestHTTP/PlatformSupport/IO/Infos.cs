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
    public abstract class FileSystemInfo
    {
        public FileAttributes Attributes
        {
            get
            {
                return this.GetAttributes();
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return this.GetCreationTime().DateTime;
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                return this.GetCreationTime().ToUniversalTime().DateTime;
            }
        }

        public abstract bool Exists { get; }

        public string Extention
        {
            get
            {
                return Path.GetExtension(this.FullName);
            }
        }

        public abstract string FullName { get; }

        public abstract string Name { get; }

        internal abstract FileAttributes GetAttributes();

        internal abstract DateTimeOffset GetCreationTime();

        public abstract void Delete();

        public void Refresh()
        {
            this.RefreshInternal();
        }

        internal abstract void RefreshInternal();
    }

    public sealed class DirectoryInfo : FileSystemInfo
    {
        private string path;
        private StorageFolder folder;

        public override bool Exists
        {
            get
            {
                try
                {
                    this.RefreshInternal();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override string FullName
        {
            get
            {
                return this.folder.Path;
            }
        }

        public override string Name
        {
            get
            {
                return this.folder.Name;
            }
        }

        public DirectoryInfo(string path)
        {
            if (path == null)
                throw new ArgumentNullException();
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException();
            try
            {
                this.path = path;
                this.folder = FileHelper.GetFolderForPathOrURI(path);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        internal DirectoryInfo(string path, StorageFolder folder)
        {
            this.path = path;
            this.folder = folder;
        }

        internal override FileAttributes GetAttributes()
        {
            try
            {
                return File.WinAttributesToSysAttributes(this.folder.Attributes);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.GetAttributes: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.GetAttributes: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        internal override DateTimeOffset GetCreationTime()
        {
            try
            {
                return this.folder.DateCreated;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.GetCreationTime: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.GetCreationTime: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public override void Delete()
        {
            try
            {
                WindowsRuntimeSystemExtensions.AsTask(this.folder.DeleteAsync()).Wait();
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.Delete: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.Delete: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        internal override void RefreshInternal()
        {
            try
            {
                this.folder = FileHelper.GetFolderForPathOrURI(this.path);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.RefreshInternal: " + ex.Message + "\n" + ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("DirectoryInfo.RefreshInternal: " + ex.Message + "\n" + ex.StackTrace);
                throw new IOException(ex.Message, ex);
            }
        }

        public override string ToString()
        {
            return this.path;
        }

        public override int GetHashCode()
        {
            return this.path.GetHashCode();
        }
    }
}
#endif