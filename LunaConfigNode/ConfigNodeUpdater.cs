using System.Collections.Generic;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Updates the given value if it exists
        /// </summary>
        public void UpdateValue(string name, string value)
        {
            if (_useDictionaryForValues)
            {
                if (ValueDict.ContainsKey(name))
                    ValueDict[name] = value;
            }
            else
            {
                for (var i = 0; i < ValueList.Count; i++)
                {
                    if (ValueList[i].Key == name)
                    {
                        ValueList[i] = new KeyValuePair<string, string>(name, value);
                    }
                }
            }
        }

        /// <summary>
        /// Updates all the nodes with the matching node name if they exist
        /// </summary>
        public void UpdateNode(ConfigNode newNode)
        {
            if (_useDictionaryForNodes)
            {
                if (NodeDict.ContainsKey(newNode.Name))
                    NodeDict[newNode.Name] = newNode;
            }
            else
            {
                for (var i = 0; i < NodeList.Count; i++)
                {
                    if (NodeList[i].Name == newNode.Name)
                    {
                        NodeList[i] = newNode;
                    }
                }
            }
        }
    }
}
