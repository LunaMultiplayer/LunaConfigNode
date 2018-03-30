using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunaConfigNode
{
    public class ConfigNode
    {
        public string Name { get; set; } = string.Empty;
        public List<ConfigNode> Nodes { get; } = new List<ConfigNode>();
        public List<Value> Values { get; } = new List<Value>();

        internal int Depth { get; set; }
        internal ConfigNode Parent { get; set; }

        #region Constructor

        public ConfigNode(string name)
        {
            Name = name;
        }

        public ConfigNode()
        {
        }

        internal ConfigNode(string name, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
            Depth = Parent.Depth;
        }

        #endregion

        #region Public

        #region Add

        public Value AddValue(string name, dynamic value)
        {
            var newValue = new Value(name, value, this);
            Values.Add(newValue);
            return newValue;
        }

        public ConfigNode AddNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);
            Nodes.Add(newConfigNode);
            return newConfigNode;
        }

        #endregion

        #region Remove

        public void RemoveValue(Value value)
        {
            Values.Remove(value);
        }

        public void RemoveValues(params Value[] values)
        {
            foreach(var val in values)
                Values.Remove(val);
        }

        public void RemoveValue(int index)
        {
            Values.RemoveAt(index);
        }

        #endregion

        #region Navigate

        public ConfigNode NavigateTo(string xpath)
        {
            var nodes = xpath.Split('/');

            if (Name == nodes[0] && nodes.Length == 1)
            {
                return this;
            }

            var node = Nodes.FirstOrDefault(n => n.Name == nodes[0]);
            return node?.NavigateTo(string.Join("/", nodes.Skip(Depth + 1)));
        }

        #endregion

        #endregion

        #region Base overrides

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Depth; i++)
            {
                builder.Append('\t');
            }
            if (Depth > 0)
            {
                builder.AppendLine(Name);
                builder.AppendLine(GetInitBracket());
            }
            foreach (var value in Values)
            {
                builder.AppendLine(value.ToString());
            }
            foreach (var node in Nodes)
            {
                builder.AppendLine(node.ToString());
            }
            if (Depth > 0)
            {
                builder.AppendLine(GetEndBracket());
            }
            return builder.ToString();
        }

        #endregion

        #region Private helpers

        private string GetInitBracket()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Depth; i++)
            {
                builder.Append('\t');
            }

            builder.AppendLine("{");
            return builder.ToString();
        }

        private string GetEndBracket()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Depth; i++)
            {
                builder.Append('\t');
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        #endregion
    }
}
