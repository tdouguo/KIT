#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
namespace Org.BouncyCastle.Asn1
{
    /**
     * basic interface for Der string objects.
     */
    public interface IAsn1String
    {
        string GetString();
    }
}

#endif
