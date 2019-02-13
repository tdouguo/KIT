#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.Collections;
using System.Text;

#if UNITY_WSA && !UNITY_EDITOR && !ENABLE_IL2CPP
using System.TypeFix;
#endif

namespace Org.BouncyCastle.Utilities.Collections
{
    public abstract class CollectionUtilities
    {
        public static void AddRange(IList to, IEnumerable range)
        {
            foreach (object o in range)
            {
                to.Add(o);
            }
        }

        public static bool CheckElementsAreOfType(IEnumerable e, Type t)
        {
            foreach (object o in e)
            {
                if (!t.IsInstanceOfType(o))
                    return false;
            }
            return true;
        }

        public static IDictionary ReadOnly(IDictionary d)
        {
            return d;
        }

        public static IList ReadOnly(IList l)
        {
            return l;
        }

        public static ISet ReadOnly(ISet s)
        {
            return s;
        }

        public static string ToString(IEnumerable c)
        {
            StringBuilder sb = new StringBuilder("[");

            IEnumerator e = c.GetEnumerator();

            if (e.MoveNext())
            {
                sb.Append(e.Current.ToString());

                while (e.MoveNext())
                {
                    sb.Append(", ");
                    sb.Append(e.Current.ToString());
                }
            }

            sb.Append(']');

            return sb.ToString();
        }
    }
}

#endif
