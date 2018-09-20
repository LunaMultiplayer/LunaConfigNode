using System;
using System.Collections.Generic;
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

        internal List<MutableKeyValue<K, V>> AllItems { get; set; } = new List<MutableKeyValue<K, V>>();
        internal HashSet<K> RepeatedKeys { get; } = new HashSet<K>();
        internal Dictionary<K, MutableKeyValue<K, V>> Dictionary { get; } = new Dictionary<K, MutableKeyValue<K, V>>();
        internal List<MutableKeyValue<K, V>> List { get; } = new List<MutableKeyValue<K, V>>();

        public MixedCollection() { }

        public MixedCollection(IEnumerable<MutableKeyValue<K, V>> collection)
        {
            lock (_lock)
            {
                var keyValuePairs = collection as List<MutableKeyValue<K, V>> ?? collection.ToList();
                AllItems.AddRange(keyValuePairs);
                foreach (var keyVal in keyValuePairs)
                {
                    if (!Dictionary.ContainsKey(keyVal.Key) && !RepeatedKeys.Contains(keyVal.Key))
                        Dictionary.Add(keyVal.Key, keyVal);
                    else
                    {
                        RepeatedKeys.Add(keyVal.Key);
                        Dictionary.Remove(keyVal.Key);
                        List.Add(keyVal);
                    }
                }
            }
        }

        public void Add(MutableKeyValue<K, V> value)
        {
            lock (_lock)
            {
                AllItems.Add(value);
                if (!RepeatedKeys.Contains(value.Key))
                {
                    if (Dictionary.ContainsKey(value.Key))
                    {
                        List.Add(Dictionary[value.Key]);
                        List.Add(value);
                        RepeatedKeys.Add(value.Key);
                        Dictionary.Remove(value.Key);
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
            var newVal = new MutableKeyValue<K, V>(key, value);
            lock (_lock)
            {
                AllItems.Add(newVal);
                if (!RepeatedKeys.Contains(key))
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        List.Add(Dictionary[key]);
                        List.Add(newVal);
                        RepeatedKeys.Add(key);
                        Dictionary.Remove(key);;
                    }
                    else
                    {
                        Dictionary.Add(key, newVal);
                    }
                }
                else
                {
                    List.Add(newVal);
                }
            }
        }

        public List<MutableKeyValue<K, V>> GetSeveral(K key)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(key) ? new List<MutableKeyValue<K, V>> { Dictionary[key] } : List.Where(k => k.Key.Equals(key)).ToList();
            }
        }

        public MutableKeyValue<K, V> GetSingle(K key)
        {
            lock (_lock)
            {
                if (!RepeatedKeys.Contains(key))
                {
                    return Dictionary.ContainsKey(key) ? Dictionary[key] : null;
                }

                throw new Exception($"Key value: \"{key}\" is not unique");
            }
        }

        public V GetSingleValue(K key)
        {
            lock (_lock)
            {
                if (!RepeatedKeys.Contains(key))
                {
                    return Dictionary.ContainsKey(key) ? Dictionary[key].Value : default(V);
                }

                throw new Exception($"Key value: \"{key}\" is not unique");
            }
        }

        public List<MutableKeyValue<K, V>> GetAll()
        {
            lock (_lock)
            {
                return AllItems;
            }
        }

        public List<K> GetAllKeys()
        {
            lock (_lock)
            {
                return AllItems.Select(v => v.Key).ToList();
            }
        }

        public List<V> GetAllValues()
        {
            lock (_lock)
            {
                return AllItems.Select(v=> v.Value).ToList();
            }
        }

        public void Update(K key, V value)
        {
            lock (_lock)
            {
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary[key].Value = value;
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
                AllItems.RemoveAll(v => v.Key.Equals(key));
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary.Remove(key);
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
                AllItems.RemoveAll(v => v.Value.Equals(value));
                if (Dictionary.ContainsKey(key))
                {
                    Dictionary.Remove(key);
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
                AllItems.RemoveAll(v => v.Equals(value));
                if (Dictionary.ContainsKey(value.Key))
                {
                    Dictionary.Remove(value.Key);
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
                return Dictionary.ContainsKey(key) || List.Any(v => v.Key.Equals(key));
            }
        }

        public bool Exists(MutableKeyValue<K, V> value)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(value.Key) || List.Contains(value);
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

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (RepeatedKeys != null ? RepeatedKeys.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Dictionary != null ? Dictionary.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (List != null ? List.GetHashCode() : 0);
                return hashCode;
            }
        }

        protected bool Equals(MixedCollection<K, V> other)
        {
            lock (_lock)
            {
                return AllItems.SequenceEqual(other.AllItems);
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
