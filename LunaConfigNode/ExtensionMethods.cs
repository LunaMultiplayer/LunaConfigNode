using System.Collections.Generic;

namespace LunaConfigNode
{
    public static class Extensions
    {
        public static IEnumerable<MutableKeyValue<K, V>> ToMutableKeyValue<K, V>(this Dictionary<K, V> dict)
        {
            var list = new List<MutableKeyValue<K, V>>();
            foreach (var v in dict)
            {
                list.Add(new MutableKeyValue<K, V>(v.Key, v.Value));
            }

            return list;
        }
    }
}
