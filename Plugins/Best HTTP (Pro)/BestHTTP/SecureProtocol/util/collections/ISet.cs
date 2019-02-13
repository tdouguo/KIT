#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)

using System;
using System.Collections;

namespace Org.BouncyCastle.Utilities.Collections
{
	public interface ISet
		: ICollection
	{
		void Add(object o);
		void AddAll(IEnumerable e);
		void Clear();
		bool Contains(object o);
		bool IsEmpty { get; }
		bool IsFixedSize { get; }
		bool IsReadOnly { get; }
		void Remove(object o);
		void RemoveAll(IEnumerable e);
	}
}

#endif
