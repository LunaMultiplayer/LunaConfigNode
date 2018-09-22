using LunaConfigNode.CfgNode;
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
        internal Dictionary<TK, CfgNodeValue<TK, TV>> SingleItems { get; } = new Dictionary<TK, CfgNodeValue<TK, TV>>();
        internal Dictionary<TK, List<CfgNodeValue<TK, TV>>> RepeatedItems { get; } = new Dictionary<TK, List<CfgNodeValue<TK, TV>>>();

        public MixedCollection() { }

        public MixedCollection(IEnumerable<CfgNodeValue<TK, TV>> collection)
        {
            lock (_lock)
            {
                Initialize(collection);
            }
        }

        public void Initialize(IEnumerable<CfgNodeValue<TK, TV>> collection)
        {
            if (!IsEmpty()) Clear();
            foreach (var keyVal in collection) Add(keyVal);
        }

        public void Add(CfgNodeValue<TK, TV> value)
        {
            lock (_lock)
            {
                AllItems.Add(value);
                if (!RepeatedItems.ContainsKey(value.Key))
                {
                    if (SingleItems.ContainsKey(value.Key))
                    {
                        RepeatedItems.Add(value.Key, new List<CfgNodeValue<TK, TV>> { SingleItems[value.Key] });
                        SingleItems.Remove(value.Key);
                        RepeatedItems[value.Key].Add(value);
                    }
                    else
                    {
                        SingleItems.Add(value.Key, value);
                    }
                }
                else
                {
                    RepeatedItems[value.Key].Add(value);
                }
            }
        }

        public List<CfgNodeValue<TK, TV>> GetSeveral(TK key)
        {
            lock (_lock)
            {
                return SingleItems.ContainsKey(key) ? new List<CfgNodeValue<TK, TV>> { SingleItems[key] } :
                    RepeatedItems.ContainsKey(key) ? RepeatedItems[key] : new List<CfgNodeValue<TK, TV>>();
            }
        }

        public CfgNodeValue<TK, TV> GetSingle(TK key)
        {
            lock (_lock)
            {
                if (!RepeatedItems.ContainsKey(key))
                {
                    return SingleItems.ContainsKey(key) ? SingleItems[key] : null;
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
                return AllItems.Select(v => v.Value).ToList();
            }
        }

        /// <summary>
        /// Sets a new value for a single element that matches in KEY
        /// </summary>
        public void Update(TK key, TV value)
        {
            lock (_lock)
            {
                if (!RepeatedItems.ContainsKey(key))
                {
                    if (SingleItems.ContainsKey(key))
                    {
                        SingleItems[key].Value = value;
                    }
                }
                else if (RepeatedItems.ContainsKey(key))
                {
                    throw new Exception($"Key value: \"{key}\" is not unique. Use Replace()");
                }
            }
        }

        /// <summary>
        /// Replaces ALL the elements that match in KEY and VALUE for the new one
        /// </summary>
        public void Replace(CfgNodeValue<TK, TV> oldKeyVal, CfgNodeValue<TK, TV> newKeyVal)
        {
            lock (_lock)
            {
                var index = AllItems.IndexOf(oldKeyVal);
                if (index >= 0)
                {
                    AllItems[index] = newKeyVal;

                    if (SingleItems.ContainsKey(oldKeyVal.Key))
                    {
                        if (oldKeyVal.Key.Equals(newKeyVal.Key))
                        {
                            SingleItems[oldKeyVal.Key] = newKeyVal;
                        }
                        else
                        {
                            SingleItems.Remove(oldKeyVal.Key);
                            SingleItems.Add(newKeyVal.Key, newKeyVal);
                        }
                    }
                    else
                    {
                        if (RepeatedItems.ContainsKey(oldKeyVal.Key))
                        {
                            if (oldKeyVal.Key.Equals(newKeyVal.Key))
                            {
                                for (var i = 0; i < RepeatedItems[oldKeyVal.Key].Count; i++)
                                {
                                    if (RepeatedItems[oldKeyVal.Key][i].Equals(oldKeyVal))
                                        RepeatedItems[oldKeyVal.Key][i] = newKeyVal;
                                }
                            }
                            else
                            {
                                Remove(oldKeyVal);
                                Add(newKeyVal);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Remove all the elements that have the given key
        /// </summary>
        public void Remove(TK key)
        {
            lock (_lock)
            {
                AllItems.RemoveAll(v => v.Key.Equals(key));
                if (SingleItems.ContainsKey(key))
                {
                    SingleItems.Remove(key);
                }
                else
                {
                    RepeatedItems.Remove(key);
                }
            }
        }

        /// <summary>
        /// Removes a specific value
        /// </summary>
        public void Remove(CfgNodeValue<TK, TV> keyVal)
        {
            lock (_lock)
            {
                AllItems.RemoveAll(v => v.Value.Equals(keyVal));
                if (SingleItems.ContainsKey(keyVal.Key))
                {
                    SingleItems.Remove(keyVal.Key);
                }
                else if (RepeatedItems.ContainsKey(keyVal.Key))
                {
                    RepeatedItems[keyVal.Key].RemoveAll(v => v.Equals(keyVal));
                    if (!RepeatedItems[keyVal.Key].Any())
                        RepeatedItems.Remove(keyVal.Key);

                }
            }
        }

        public bool Exists(TK key)
        {
            lock (_lock)
            {
                return SingleItems.ContainsKey(key) || RepeatedItems.ContainsKey(key);
            }
        }

        public bool Exists(CfgNodeValue<TK, TV> keyVal)
        {
            lock (_lock)
            {
                return SingleItems.ContainsKey(keyVal.Key) || RepeatedItems.ContainsKey(keyVal.Key) && RepeatedItems[keyVal.Key].Contains(keyVal);
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
                SingleItems.Clear();
                RepeatedItems.Clear();
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
                hashCode = (hashCode * 397) ^ (SingleItems != null ? SingleItems.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (RepeatedItems != null ? RepeatedItems.GetHashCode() : 0);
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
