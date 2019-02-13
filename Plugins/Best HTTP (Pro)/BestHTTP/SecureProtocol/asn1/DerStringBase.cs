#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Org.BouncyCastle.Asn1
{
	public abstract class DerStringBase
		: Asn1Object, IAsn1String
	{
		protected DerStringBase()
		{
		}

		public abstract string GetString();

		public override string ToString()
		{
			return GetString();
		}

		protected override int Asn1GetHashCode()
		{
			return GetString().GetHashCode();
		}
	}
}

#endif
