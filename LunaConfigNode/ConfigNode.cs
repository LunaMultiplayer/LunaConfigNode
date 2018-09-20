using System;
using System.IO;
using System.Text;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        private const string ValueSeparator = " = ";
        private const string OpenNodeSymbol = "{";
        private const string CloseNodeSymbol = "}";

        public string Name { get; set; } = string.Empty;
        public ConfigNode Parent { get; set; }

        private MixedCollection<string, string> ValueDict { get; set; } = new MixedCollection<string, string>();
        private MixedCollection<string, ConfigNode> NodeDict { get; set; } = new MixedCollection<string, ConfigNode>();

        private int Depth => Parent?.Depth + 1 ?? 0;

        #region Constructor

        public ConfigNode()
        {
        }

        public ConfigNode(string contents)
        {
            var currentNode = this;
            using (var reader = new StringReader(contents))
            {
                var previousLine = string.Empty;
                string line;
                while ((line = reader.ReadLine()?.TrimStart()) != null)
                {
                    if (line.Contains(ValueSeparator))
                    {
                        currentNode.AddValue(line.Substring(0, line.IndexOf(ValueSeparator, StringComparison.Ordinal)).Trim(),
                            line.Substring(line.LastIndexOf(ValueSeparator, StringComparison.Ordinal) + ValueSeparator.Length).Trim());

                        continue;
                    }
                    if (line.TrimEnd().Equals(OpenNodeSymbol))
                    {
                        currentNode = currentNode.AddNode(previousLine);
                        continue;
                    }
                    if (line.TrimEnd().Equals(CloseNodeSymbol))
                    {
                        currentNode = currentNode.Parent;
                        continue;
                    }
                    previousLine = line;
                }
            }
        }

        internal ConfigNode(string name, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
        }

        #endregion
        
        #region Base overrides

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Depth > 0) //When we are in a subnode initialize it
            {
                InitializeNode(builder);
            }

            foreach (var value in ValueDict.GetAll())
            {
                GetFieldTabbing(builder);
                builder.Append(value.Key);
                builder.Append(ValueSeparator);
                builder.AppendLine(value.Value);
            }

            foreach (var value in NodeDict.GetAllValues())
            {
                builder.AppendLine(value.ToString());
            }

            if (Depth > 0)
            {
                FinishNode(builder);
            }
            return builder.ToString().TrimEnd();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj.GetType() == typeof(ConfigNode) && Equals((ConfigNode)obj);
        }

        protected bool Equals(ConfigNode other)
        {
            return string.Equals(Name, other.Name) && ValueDict.Equals(other.ValueDict) && NodeDict.Equals(other.NodeDict);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Parent != null ? Parent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValueDict != null ? ValueDict.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (NodeDict != null ? NodeDict.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ConfigNode lhs, ConfigNode rhs)
        {
            if (lhs == null)
            {
                return rhs == null;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ConfigNode lhs, ConfigNode rhs) => !(lhs == rhs);

        #endregion

        #region Private helpers

        private void GetFieldTabbing(StringBuilder builder)
        {
            for (var i = 0; i < Depth; i++)
            {
                builder.Append('\t');
            }
        }

        private void GetNodeTabbing(StringBuilder builder)
        {
            for (var i = 0; i < Depth - 1; i++)
            {
                builder.Append('\t');
            }
        }

        private void InitializeNode(StringBuilder builder)
        {
            GetNodeTabbing(builder);
            builder.AppendLine(Name);
            GetNodeTabbing(builder);
            builder.AppendLine(OpenNodeSymbol);
        }

        private void FinishNode(StringBuilder builder)
        {
            GetNodeTabbing(builder);
            builder.Append(CloseNodeSymbol);
        }

        #endregion
    }
}
