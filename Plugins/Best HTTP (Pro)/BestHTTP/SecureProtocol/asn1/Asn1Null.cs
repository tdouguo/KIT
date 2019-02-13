#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Org.BouncyCastle.Asn1
{
    /**
     * A Null object.
     */
    public abstract class Asn1Null
        : Asn1Object
    {
        internal Asn1Null()
        {
        }

		public override string ToString()
		{
			return "NULL";
		}
    }
}

#endif
