#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || NETFX_CORE || PORTABLE)
    [Serializable]
#endif
    public class PemGenerationException
		: Exception
	{
		public PemGenerationException()
			: base()
		{
		}

		public PemGenerationException(
			string message)
			: base(message)
		{
		}

		public PemGenerationException(
			string		message,
			Exception	exception)
			: base(message, exception)
		{
		}
	}
}

#endif
