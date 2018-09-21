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

        internal List<CfgNodeValue<K, V>> AllItems { get; } = new List<CfgNodeValue<K, V>>();
        internal Dictionary<K, int> RepeatedKeys { get; } = new Dictionary<K, int>();
        internal Dictionary<K, CfgNodeValue<K, V>> Dictionary { get; } = new Dictionary<K, CfgNodeValue<K, V>>();
        internal List<CfgNodeValue<K, V>> List { get; } = new List<CfgNodeValue<K, V>>();

        public MixedCollection() { }

        public MixedCollection(IEnumerable<CfgNodeValue<K, V>> collection)
        {
            lock (_lock)
            {
                var keyValuePairs = collection as List<CfgNodeValue<K, V>> ?? collection.ToList();
                AllItems.AddRange(keyValuePairs);
                foreach (var keyVal in keyValuePairs)
                {
                    if (!Dictionary.ContainsKey(keyVal.Key) && !RepeatedKeys.ContainsKey(keyVal.Key))
                        Dictionary.Add(keyVal.Key, keyVal);
                    else
                    {
                        if (RepeatedKeys.ContainsKey(keyVal.Key))
                            RepeatedKeys[keyVal.Key]++;
                        else
                            RepeatedKeys.Add(keyVal.Key, 1);

                        Dictionary.Remove(keyVal.Key);
                        List.Add(keyVal);
                    }
                }
            }
        }

        public void Add(CfgNodeValue<K, V> value)
        {
            lock (_lock)
            {
                AllItems.Add(value);
                if (!RepeatedKeys.ContainsKey(value.Key))
                {
                    if (Dictionary.ContainsKey(value.Key))
                    {
                        List.Add(Dictionary[value.Key]);
                        List.Add(value);
                        RepeatedKeys.Add(value.Key, 1);
                        Dictionary.Remove(value.Key);
                    }
                    else
                    {
                        Dictionary.Add(value.Key, value);
                    }
                }
                else
                {
                    RepeatedKeys[value.Key]++;
                    List.Add(value);
                }
            }
        }

        public void Create(K key, V value)
        {
            var newVal = new CfgNodeValue<K, V>(key, value);
            lock (_lock)
            {
                AllItems.Add(newVal);
                if (!RepeatedKeys.ContainsKey(key))
                {
                    if (Dictionary.ContainsKey(key))
                    {
                        List.Add(Dictionary[key]);
                        List.Add(newVal);
                        RepeatedKeys.Add(key, 1);
                        Dictionary.Remove(key);;
                    }
                    else
                    {
                        Dictionary.Add(key, newVal);
                    }
                }
                else
                {
                    RepeatedKeys[key]++;
                    List.Add(newVal);
                }
            }
        }

        public List<CfgNodeValue<K, V>> GetSeveral(K key)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(key) ? new List<CfgNodeValue<K, V>> { Dictionary[key] } : List.Where(k => k.Key.Equals(key)).ToList();
            }
        }

        public CfgNodeValue<K, V> GetSingle(K key)
        {
            lock (_lock)
            {
                if (!RepeatedKeys.ContainsKey(key))
                {
                    return Dictionary.ContainsKey(key) ? Dictionary[key] : null;
                }

                throw new Exception($"Key value: \"{key}\" is not unique");
            }
        }

        public List<CfgNodeValue<K, V>> GetAll()
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
                    RepeatedKeys.Remove(key);
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
                    var numberRemoved = List.RemoveAll(v => v.Value.Equals(value));
                    if (RepeatedKeys[key] == numberRemoved)
                        RepeatedKeys.Remove(key);
                    else
                        RepeatedKeys[key] -= numberRemoved;
                }
            }
        }

        public void Remove(CfgNodeValue<K, V> value)
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
                    RepeatedKeys.Remove(value.Key);
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

        public bool Exists(CfgNodeValue<K, V> value)
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
                var hashCode = (AllItems != null ? AllItems.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RepeatedKeys != null ? RepeatedKeys.GetHashCode() : 0);
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
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return !ReferenceEquals(rhs, null) && lhs.Equals(rhs);
        }

        public static bool operator !=(MixedCollection<K, V> lhs, MixedCollection<K, V> rhs) => !(lhs == rhs);

        #endregion
    }
}
