#if (UNITY_WSA || BUILD_FOR_WP8) && !UNITY_EDITOR && !ENABLE_IL2CPP

using System;

using Windows.Storage.Streams;

namespace BestHTTP.PlatformSupport.TcpClient.WinRT
{
    public sealed class DataReaderWriterStream : System.IO.Stream
    {
        private TcpClient Client { get; set; }
        private DataReader Reader { get; set; }
        private DataWriter Writer { get; set; }

        public DataReaderWriterStream(TcpClient socket)
        {
            this.Client = socket;
            this.Reader = new DataReader(Client.Socket.InputStream);
            this.Writer = new DataWriter(Client.Socket.OutputStream);
        }

#region Stream interface

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            Writer.StoreAsync().AsTask().Wait();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool DataAvailable
        {
            get
            {
                return Reader.UnconsumedBufferLength > 0;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Windows.Storage.Streams.Buffer tmpBuffer = new Windows.Storage.Streams.Buffer((uint)count);

            try
            {
                var task = Client.Socket.InputStream.ReadAsync(tmpBuffer, (uint)count, InputStreamOptions.None);
                task.AsTask().Wait();
            }
            catch(AggregateException ex)
            {
                if (ex.InnerException != null)
                    throw ex.InnerException;
                else
                    throw ex;
            }

            /*byte[] tmpBuff = tmpBuffer.ToArray();
            int length = Math.Min(tmpBuff.Length, count);

            Array.Copy(tmpBuff, 0, buffer, offset, length);

            return length;*/
            
            DataReader buf = DataReader.FromBuffer(tmpBuffer);
            int length = (int)buf.UnconsumedBufferLength;
            for (int i = 0; i < length; ++i)
                buffer[offset + i] = buf.ReadByte();
            return length;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; ++i)
                Writer.WriteByte(buffer[offset + i]);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

#endregion

#region Dispose

        private bool disposed = false;
        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (Reader != null)
                    {
                        Reader.Dispose();
                        Reader = null;
                    }

                    if (Writer != null)
                    {
                        Writer.Dispose();
                        Writer = null;
                    }
                }
                disposed = true;
            }
            base.Dispose(disposing);
        }

#endregion
    }
}

#endif