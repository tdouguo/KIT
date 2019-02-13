#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
    public interface GlvEndomorphism
        :   ECEndomorphism
    {
        BigInteger[] DecomposeScalar(BigInteger k);
    }
}

#endif
