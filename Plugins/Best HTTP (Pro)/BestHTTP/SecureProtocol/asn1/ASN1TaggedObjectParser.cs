#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Org.BouncyCastle.Asn1
{
	public interface Asn1TaggedObjectParser
		: IAsn1Convertible
	{
		int TagNo { get; }

		IAsn1Convertible GetObjectParser(int tag, bool isExplicit);
	}
}

#endif
