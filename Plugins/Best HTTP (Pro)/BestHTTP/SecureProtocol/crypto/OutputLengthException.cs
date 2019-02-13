#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Crypto
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || NETFX_CORE || PORTABLE)
    [Serializable]
#endif
    public class OutputLengthException
        : DataLengthException
    {
        public OutputLengthException()
        {
        }

        public OutputLengthException(
            string message)
            : base(message)
        {
        }

        public OutputLengthException(
            string message,
            Exception exception)
            : base(message, exception)
        {
        }
    }
}

#endif
