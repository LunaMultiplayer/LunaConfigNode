using System.Collections.Generic;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Adds the specified name and value
        /// </summary>
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

        /// <summary>
        /// Adds a new config node with the given name
        /// </summary>
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
    }
}
