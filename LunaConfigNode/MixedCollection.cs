using System;
using System.Collections.Generic;
using System.Linq;

namespace LunaConfigNode
{
    /// <summary>
    /// This is a thread safe collection that uses a Dictionary when possible. Otherwise it uses a List
    /// Provides fast access to the elements if they are not repeated (as it will use the dictionary for looking it up)
    /// The structure is 'Key,Value' as in a dictionary
    /// </summary>
    public class MixedCollection<TK, TV>
    {
        private readonly object _lock = new object();

        internal List<CfgNodeValue<TK, TV>> AllItems { get; } = new List<CfgNodeValue<TK, TV>>();
        internal Dictionary<TK, int> RepeatedKeys { get; } = new Dictionary<TK, int>();
        internal Dictionary<TK, CfgNodeValue<TK, TV>> Dictionary { get; } = new Dictionary<TK, CfgNodeValue<TK, TV>>();
        internal List<CfgNodeValue<TK, TV>> List { get; } = new List<CfgNodeValue<TK, TV>>();

        public MixedCollection() { }

        public MixedCollection(IEnumerable<CfgNodeValue<TK, TV>> collection)
        {
            lock (_lock)
            {
                Initialize(collection);
            }
        }

        public void Add(CfgNodeValue<TK, TV> value)
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

        public void Create(TK key, TV value)
        {
            var newVal = new CfgNodeValue<TK, TV>(key, value);
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
                        Dictionary.Remove(key);
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

        public List<CfgNodeValue<TK, TV>> GetSeveral(TK key)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(key) ? new List<CfgNodeValue<TK, TV>> { Dictionary[key] } : List.Where(k => k.Key.Equals(key)).ToList();
            }
        }

        public CfgNodeValue<TK, TV> GetSingle(TK key)
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

        public List<CfgNodeValue<TK, TV>> GetAll()
        {
            lock (_lock)
            {
                return AllItems;
            }
        }

        public List<TK> GetAllKeys()
        {
            lock (_lock)
            {
                return AllItems.Select(v => v.Key).ToList();
            }
        }

        public List<TV> GetAllValues()
        {
            lock (_lock)
            {
                return AllItems.Select(v=> v.Value).ToList();
            }
        }

        /// <summary>
        /// Sets a new value for all the elements that matches in KEY
        /// </summary>
        public void Update(TK key, TV value)
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

        /// <summary>
        /// Replaces all the elements that match in KEY and VALUE for the new one
        /// </summary>
        public void Replace(CfgNodeValue<TK, TV> oldValue, CfgNodeValue<TK, TV> newValue)
        {
            lock (_lock)
            {
                if (Dictionary.ContainsKey(oldValue.Key))
                {
                    Dictionary[oldValue.Key] = newValue;
                }
                else
                {
                    for (var i = 0; i < List.Count; i++)
                    {
                        if (List[i].Equals(oldValue))
                        {
                            List[i] = newValue;
                        }
                    }
                }
            }
        }

        public void Remove(TK key)
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

        public void Remove(TK key, TV value)
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

        public void Remove(CfgNodeValue<TK, TV> value)
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
                    var numberRemoved = List.RemoveAll(v => v.Equals(value));
                    if (RepeatedKeys[value.Key] == numberRemoved)
                        RepeatedKeys.Remove(value.Key);
                    else
                        RepeatedKeys[value.Key] -= numberRemoved;
                }
            }
        }

        public bool Exists(TK key)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(key) || List.Any(v => v.Key.Equals(key));
            }
        }

        public bool Exists(CfgNodeValue<TK, TV> value)
        {
            lock (_lock)
            {
                return Dictionary.ContainsKey(value.Key) || List.Contains(value);
            }
        }

        public int Count()
        {
            lock (_lock)
            {
                return AllItems.Count;
            }
        }

        public bool IsEmpty()
        {
            lock (_lock)
            {
                return AllItems.Count == 0;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                AllItems.Clear();
                Dictionary.Clear();
                List.Clear();
                RepeatedKeys.Clear();
            }
        }

        public void Initialize(IEnumerable<CfgNodeValue<TK, TV>> collection)
        {
            if (!IsEmpty()) Clear();

            var keyValuePairs = collection as List<CfgNodeValue<TK, TV>> ?? collection.ToList();
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

        #region Base overrides

        public override bool Equals(object obj)
        {
            lock (_lock)
            {
                if (obj == null) return false;
                return obj.GetType() == typeof(MixedCollection<TK, TV>) && Equals((MixedCollection<TK, TV>)obj);
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


        protected bool Equals(MixedCollection<TK, TV> other)
        {
            lock (_lock)
            {
                return AllItems.SequenceEqual(other.AllItems);
            }
        }


        public static bool operator ==(MixedCollection<TK, TV> lhs, MixedCollection<TK, TV> rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return !ReferenceEquals(rhs, null) && lhs.Equals(rhs);
        }

        public static bool operator !=(MixedCollection<TK, TV> lhs, MixedCollection<TK, TV> rhs) => !(lhs == rhs);

        #endregion
    }
}
