using System.Collections.Generic;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Returns all the values inside this ConfigNode
        /// </summary>
        public List<MutableKeyValue<string, string>> GetAllValues()
        {
            return ValueDict.GetAll();
        }

        /// <summary>
        /// Returns all the nodes inside this ConfigNode
        /// </summary>
        public List<ConfigNode> GetAllNodes()
        {
            return NodeDict.GetAllValues();
        }

        /// <summary>
        /// Returns all the values with the specified name
        /// </summary>
        public List<MutableKeyValue<string, string>> GetValues(string name)
        {
            return ValueDict.Get(name);
        }

        /// <summary>
        /// Returns all the config nodes with the specified name
        /// </summary>
        public List<MutableKeyValue<string, ConfigNode>> GetNodes(string nodeName)
        {
            return NodeDict.Get(nodeName);
        }
    }
}
