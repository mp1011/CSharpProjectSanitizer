using System.Collections.Generic;

namespace ProjectSanitizer.Base.Extensions
{
    public static class DictionaryExtensions
    {
        public static V TryGet<K,V>(this Dictionary<K,V> dictionary, K key)
            where V:class
        {
            V val;
            if (!dictionary.TryGetValue(key, out val))
                return null;
            else
                return val;
        }
    }
}
