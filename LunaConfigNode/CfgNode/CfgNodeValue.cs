using System.Collections.Generic;

namespace LunaConfigNode
{
    public class CfgNodeValue<T1, T2>
    {
        private const string ValueSeparator = " = ";

        public T1 Key { get; set; }
        public T2 Value { get; set; }

        public CfgNodeValue(T1 first, T2 second)
        {
            Key = first;
            Value = second;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return obj.GetType() == typeof(CfgNodeValue<T1, T2>) && Equals((CfgNodeValue<T1, T2>)obj);
        }

        protected bool Equals(CfgNodeValue<T1, T2> other)
        {
            return Key.Equals(other.Key) && Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return (EqualityComparer<T1>.Default.GetHashCode(Key) * 397) ^ EqualityComparer<T2>.Default.GetHashCode(Value);
        }

        public static bool operator ==(CfgNodeValue<T1, T2> lhs, CfgNodeValue<T1, T2> rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return !ReferenceEquals(rhs, null) && lhs.Equals(rhs);
        }

        public static bool operator !=(CfgNodeValue<T1, T2> lhs, CfgNodeValue<T1, T2> rhs) => !(lhs == rhs);

        public override string ToString()
        {
            //No need to use a stringbuilder, the compiler will do it for us
            return Key + ValueSeparator + Value;
        }
    }
}
