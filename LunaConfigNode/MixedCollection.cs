﻿using System.Collections.Generic;
using System.Linq;

namespace LunaConfigNode
{
    /// <summary>
    /// This is a thread safe collection that uses a Dictionary when possible. Otherwise it uses a List
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    public class MixedCollection<K, V>
    {
        private readonly object _lock = new object();

        internal bool UseDictionary = true;
        internal Dictionary<K, MutableKeyValue<K, V>> Dictionary { get; private set; } = new Dictionary<K, MutableKeyValue<K, V>>();
        internal List<MutableKeyValue<K, V>> List { get; private set; }

        public MixedCollection() { }

        public MixedCollection(IEnumerable<MutableKeyValue<K, V>> collection)
        {
            lock (_lock)
            {
                var keyValuePairs = collection as List<MutableKeyValue<K, V>> ?? collection.ToList();

                UseDictionary = !keyValuePairs.GroupBy(x => x.Key).Any(x => x.Count() > 1);
                if (UseDictionary)
                {
                    Dictionary = keyValuePairs.ToDictionary(k => k.Key, v => v);
                }
                else
                {
                    List = keyValuePairs.ToList();
                }
            }
        }

        public void Add(MutableKeyValue<K, V> value)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(value.Key))
                    {
                        UseDictionary = false;
                        List = new List<MutableKeyValue<K, V>>(Dictionary.ToMutableKeyValue()) { value };
                        Dictionary = null;
                    }
                    else
                    {
                        Dictionary.Add(value.Key, value);
                    }
                }
                else
                {
                    List.Add(value);
                }
            }
        }

        public void Create(K key, V value)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        UseDictionary = false;
                        List = new List<MutableKeyValue<K, V>>(Dictionary.ToMutableKeyValue()) { new MutableKeyValue<K, V>(key, value) };
                        Dictionary = null;
                    }
                    else
                    {
                        Dictionary.Add(key, new MutableKeyValue<K, V>(key, value));
                    }
                }
                else
                {
                    List.Add(new MutableKeyValue<K, V>(key, value));
                }
            }
        }

        public List<MutableKeyValue<K, V>> Get(K key)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        return new List<MutableKeyValue<K, V>> { Dictionary[key] };
                    }
                }
                else
                {
                    return List.Where(k => k.Key.Equals(key)).ToList();
                }

                return new List<MutableKeyValue<K, V>>();
            }
        }

        public List<MutableKeyValue<K, V>> GetAll()
        {
            lock (_lock)
            {
                return UseDictionary ? Dictionary.ToMutableKeyValue().ToList() : List;
            }
        }

        public List<K> GetAllKeys()
        {
            lock (_lock)
            {
                return UseDictionary ? Dictionary.ToMutableKeyValue().Select(v => v.Key).ToList() : List.Select(v => v.Key).ToList();
            }
        }

        public List<V> GetAllValues()
        {
            lock (_lock)
            {
                return UseDictionary ? Dictionary.ToMutableKeyValue().Select(v => v.Value).ToList() : List.Select(v => v.Value).ToList();
            }
        }

        public void Update(K key, V value)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        Dictionary[key].Value = value;
                    }
                }
                else
                {
                    foreach (var keyVal in List.Where(k => k.Key.Equals(key)))
                    {
                        keyVal.Value = value;
                    }
                }
            }
        }

        public void Remove(K key)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        Dictionary.Remove(key);
                    }
                }
                else
                {
                    List.RemoveAll(v => v.Key.Equals(key));
                }
            }
        }

        public void Remove(K key, V value)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        Dictionary.Remove(key);
                    }
                }
                else
                {
                    List.RemoveAll(v => v.Value.Equals(value));
                }
            }
        }

        public void Remove(MutableKeyValue<K, V> value)
        {
            lock (_lock)
            {
                if (UseDictionary)
                {
                    if (Dictionary.ContainsKey(value.Key))
                    {
                        Dictionary.Remove(value.Key);
                    }
                }
                else
                {
                    List.RemoveAll(v => v.Equals(value));
                }
            }
        }

        public bool Exists(K key)
        {
            lock (_lock)
            {
                return UseDictionary ? Dictionary.ContainsKey(key) : List.Any(v => v.Key.Equals(key));
            }
        }

        public bool Exists(MutableKeyValue<K, V> value)
        {
            lock (_lock)
            {
                return UseDictionary ? Dictionary.ContainsKey(value.Key) : List.Contains(value);
            }
        }

        #region Base overrides

        public override bool Equals(object obj)
        {
            lock (_lock)
            {
                if (obj == null) return false;
                return obj.GetType() == typeof(MixedCollection<K, V>) && Equals((MixedCollection<K, V>)obj);
            }
        }

        protected bool Equals(MixedCollection<K, V> other)
        {
            lock (_lock)
            {
                if (UseDictionary != other.UseDictionary) return false;
                return UseDictionary ? Dictionary.SequenceEqual(other.Dictionary) : List.SequenceEqual(other.List);
            }
        }

        public override int GetHashCode()
        {
            lock (_lock)
            {
                unchecked
                {
                    var hashCode = UseDictionary.GetHashCode();
                    hashCode = (hashCode * 397) ^ (Dictionary != null ? Dictionary.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (List != null ? List.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        public static bool operator ==(MixedCollection<K, V> lhs, MixedCollection<K, V> rhs)
        {
            if (lhs == null)
            {
                return rhs == null;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(MixedCollection<K, V> lhs, MixedCollection<K, V> rhs) => !(lhs == rhs);

        #endregion
    }
}