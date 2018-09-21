using System;
using System.IO;

namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        public string Name { get; set; } = string.Empty;
        public ConfigNode Parent { get; private set; }

        public MixedCollection<string, string> Values { get; } = new MixedCollection<string, string>();
        public MixedCollection<string, ConfigNode> Nodes { get; } = new MixedCollection<string, ConfigNode>();

        public int Depth => Parent?.Depth + 1 ?? 0;

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
                    if (line.Contains(CfgNodeConstants.ValueSeparator))
                    {
                        currentNode.AddValue(line.Substring(0, line.IndexOf(CfgNodeConstants.ValueSeparator, StringComparison.Ordinal)).Trim(),
                            line.Substring(line.LastIndexOf(CfgNodeConstants.ValueSeparator, StringComparison.Ordinal) + CfgNodeConstants.ValueSeparator.Length).Trim());

                        continue;
                    }
                    if (line.TrimEnd().Equals(CfgNodeConstants.OpenNodeSymbol))
                    {
                        currentNode = currentNode.AddNode(previousLine);
                        continue;
                    }
                    if (line.TrimEnd().Equals(CfgNodeConstants.CloseNodeSymbol))
                    {
                        currentNode = currentNode.Parent;
                        continue;
                    }
                    previousLine = line;
                }
            }
        }

        private ConfigNode(string name, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
        }

        #endregion
        
        #region Base overrides

        public override string ToString()
        {
            return CfgNodeWriter.WriteConfigNode(this);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            return obj.GetType() == typeof(ConfigNode) && Equals((ConfigNode)obj);
        }

        protected bool Equals(ConfigNode other)
        {
            return string.Equals(Name, other.Name) && Values.Equals(other.Values) && Nodes.Equals(other.Nodes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Values.GetHashCode();
                hashCode = (hashCode * 397) ^ Nodes.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(ConfigNode lhs, ConfigNode rhs)
        {
            if (ReferenceEquals(lhs, null))
            {
                return ReferenceEquals(rhs, null);
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ConfigNode lhs, ConfigNode rhs) => !(lhs == rhs);

        #endregion
    }
}
