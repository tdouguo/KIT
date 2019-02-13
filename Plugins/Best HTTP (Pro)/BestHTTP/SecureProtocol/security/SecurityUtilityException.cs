#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Security
{
#if !(NETCF_1_0 || NETCF_2_0 || SILVERLIGHT || NETFX_CORE || PORTABLE)
    [Serializable]
#endif
    public class SecurityUtilityException
		: Exception
    {
        /**
        * base constructor.
        */
        public SecurityUtilityException()
        {
        }

		/**
         * create a SecurityUtilityException with the given message.
         *
         * @param message the message to be carried with the exception.
         */
        public SecurityUtilityException(
            string message)
			: base(message)
        {
        }

		public SecurityUtilityException(
            string		message,
            Exception	exception)
			: base(message, exception)
        {
        }
    }
}

#endif
