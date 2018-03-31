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

        internal int Depth => Parent?.Depth + 1 ?? 0;
        internal ConfigNode Parent { get; set; }

        #region Constructor

        public ConfigNode(string name) => Name = name;

        public ConfigNode()
        {
        }

        internal ConfigNode(string name, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
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

        public bool TryGetValue(out Value value, params string[] xpath)
        {
            value = null;
            if (xpath.Length == 2)
            {
                value = Values.FirstOrDefault(v => v.Name == xpath[0]);
                if (value != null)
                {
                    return value.Val == xpath.Last();
                }
                return false;
            }

            var shortXpath = xpath.Skip(1).ToArray();
            foreach (var node in Nodes)
            {
                if (node.TryGetValue(out value, shortXpath))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region Base overrides

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Depth > 0)
            {
                builder.AppendLine(GetTabbedName());
                builder.AppendLine(GetInitBracket());
            }
            foreach (var value in Values)
            {
                builder.AppendLine(value.ToString());
            }
            for (var i = 0; i < Nodes.Count; i++)
            {
                builder.AppendLine(Nodes[i].ToString());
            }
            if (Depth > 0)
            {
                builder.AppendLine(GetEndBracket());
            }
            return builder.ToString().TrimEnd();
        }

        #endregion

        #region Private helpers

        private string GetTabbedName()
        {
            var builder = new StringBuilder();
            for (var i = 1; i < Depth; i++)
            {
                builder.Append('\t');
            }
            builder.Append(Name);
            return builder.ToString();
        }

        private string GetInitBracket()
        {
            var builder = new StringBuilder();
            for (var i = 1; i < Depth; i++)
            {
                builder.Append('\t');
            }

            builder.Append("{");
            return builder.ToString();
        }

        private string GetEndBracket()
        {
            var builder = new StringBuilder();
            for (var i = 1; i < Depth; i++)
            {
                builder.Append('\t');
            }

            builder.Append("}");
            return builder.ToString();
        }

        #endregion
    }
}
