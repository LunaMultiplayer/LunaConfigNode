using System.Collections.Generic;
using System.Linq;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Returns all the values inside this ConfigNode
        /// </summary>
        public KeyValuePair<string, string>[] GetAllValues()
        {
            if (_useDictionaryForValues)
            {
                return ValueDict.ToArray();
            }

            return ValueList.ToArray();
        }

        /// <summary>
        /// Returns all the nodes inside this ConfigNode
        /// </summary>
        public ConfigNode[] GetAllNodes()
        {
            if (_useDictionaryForNodes)
            {
                return NodeDict.Values.ToArray();
            }

            return NodeList.ToArray();
        }

        /// <summary>
        /// Returns the specified value
        /// </summary>
        public string GetValue(string name)
        {
            if (_useDictionaryForValues && ValueDict.TryGetValue(name, out var value))
            {
                return value;
            }

            if (ValueList.Any(v => v.Key == name))
            {
                return ValueList.FirstOrDefault(v => v.Key == name).Value;
            }

            return null;
        }

        /// <summary>
        /// Returns all the config nodes with the specified name
        /// </summary>
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
    }
}
