using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LunaConfigNode
{
    public class ConfigNode
    {
        public string Name { get; set; } = string.Empty;
        public ConfigNode Parent { get; set; }

        private bool _useDictionaryForValues = true;
        private Dictionary<string, string> ValueDict { get; } = new Dictionary<string, string>();
        private List<KeyValuePair<string,string>> ValueList { get; } = new List<KeyValuePair<string, string>>();

        private bool _useDictionaryForNodes = true;
        private Dictionary<string, ConfigNode> NodeDict { get; } = new Dictionary<string, ConfigNode>();
        private List<ConfigNode> NodeList { get; } = new List<ConfigNode>();

        private int Depth => Parent?.Depth + 1 ?? 0;

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

        public void AddValue(string name, string value)
        {
            if (_useDictionaryForValues)
            {
                if (ValueDict.ContainsKey(name))
                {
                    _useDictionaryForValues = false;
                    ValueList.AddRange(ValueDict);
                    ValueList.Add(new KeyValuePair<string, string>(name, value));
                    ValueDict.Clear();
                }
                else
                {
                    ValueDict.Add(name, value);
                }
            }
            else
            {
                ValueList.Add(new KeyValuePair<string, string>(name, value));
            }
        }

        public ConfigNode AddNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);

            if (_useDictionaryForNodes)
            {
                if (NodeDict.ContainsKey(name))
                {
                    _useDictionaryForNodes = false;
                    NodeList.AddRange(NodeDict.Values);
                    NodeList.Add(newConfigNode);
                    NodeDict.Clear();
                }
                else
                {
                    NodeDict.Add(name, newConfigNode);
                }
            }
            else
            {
                NodeList.Add(newConfigNode);
            }

            return newConfigNode;
        }

        #endregion

        #region Remove

        public void RemoveValue(string name)
        {
            if (_useDictionaryForValues)
            {
                ValueDict.Remove(name);
            }
            else
            {
                ValueList.RemoveAll(v => v.Key == name);
            }
        }

        public ConfigNode RemoveNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);

            if (_useDictionaryForNodes)
            {
                NodeDict.Remove(name);
            }
            else
            {
                NodeList.RemoveAll(c=> c.Name == name);
            }

            return newConfigNode;
        }

        #endregion

        #region Navigate

        public string GetValue(string name)
        {
            if (_useDictionaryForValues && ValueDict.TryGetValue(name, out var value))
            {
                return value;
            }

            var keyVal = ValueList.FirstOrDefault(v => v.Key == name);
            return keyVal.Value;
        }

        public List<ConfigNode> GetNodes(string nodeName)
        {
            var nodes = new List<ConfigNode>();
            if (_useDictionaryForNodes)
            {
                if (NodeDict.TryGetValue(nodeName, out var foundNode))
                    nodes.Add(foundNode);
            }
            else
            {
                nodes.AddRange(NodeList.Where(n => n.Name == nodeName));
            }

            return nodes;
        }

        public bool Navigate(out ConfigNode node, params string[] xpath)
        {
            node = null;
            if (xpath.Length == 2)
            {
                if (TryGetValue(xpath[0], out var value) && value == xpath[1])
                {
                    node = this;
                    return true;
                }
            }

            if (_useDictionaryForNodes)
            {
                if (NodeDict.TryGetValue(xpath[0], out var foundNode))
                    return foundNode.Navigate(out node, xpath.Skip(1).ToArray());
            }
            else
            {
                foreach (var possibleNode in NodeList.Where(n => n.Name == xpath[0]))
                {
                    if (possibleNode.Navigate(out node, xpath.Skip(1).ToArray()))
                        return true;
                }
            }

            return false;
        }

        public bool TryGetValue(string name, out string value)
        {
            value = null;
            if (_useDictionaryForValues)
            {
                return ValueDict.TryGetValue(name, out value);
            }

            var keyVal = ValueList.FirstOrDefault(v => v.Key == name);
            value = keyVal.Value;

            return keyVal.Key == name;
        }

        public bool TryGetNodes(string nodeName, out List<ConfigNode> nodes )
        {
            nodes = new List<ConfigNode>();
            if (_useDictionaryForNodes)
            {
                if (NodeDict.TryGetValue(nodeName, out var foundNode))
                    nodes.Add(foundNode);
            }
            else
            {
                nodes.AddRange(NodeList.Where(n => n.Name == nodeName));
            }

            return nodes.Any();
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

            if (_useDictionaryForValues)
            {
                foreach (var valueDictValue in ValueDict.Values)
                {
                    builder.AppendLine(valueDictValue);
                }
            }
            else
            {
                foreach (var value in ValueList)
                {
                    builder.AppendLine(value.ToString());
                }
            }

            if (_useDictionaryForNodes)
            {
                foreach (var nodeDictValue in NodeDict.Values)
                {
                    builder.AppendLine(nodeDictValue.ToString());
                }
            }
            else
            {
                for (var i = 0; i < NodeList.Count; i++)
                {
                    builder.AppendLine(NodeList[i].ToString());
                }
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
