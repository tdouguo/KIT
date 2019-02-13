#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Security
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || NETFX_CORE || PORTABLE)
    [Serializable]
#endif
    public class InvalidKeyException : KeyException
	{
		public InvalidKeyException() : base() { }
		public InvalidKeyException(string message) : base(message) { }
		public InvalidKeyException(string message, Exception exception) : base(message, exception) { }
	}
}

#endif
