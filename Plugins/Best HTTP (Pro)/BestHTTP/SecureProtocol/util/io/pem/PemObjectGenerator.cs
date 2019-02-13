#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	public interface PemObjectGenerator
	{
		/// <returns>
		/// A <see cref="PemObject"/>
		/// </returns>
		/// <exception cref="PemGenerationException"></exception>
		PemObject Generate();
	}
}

#endif
