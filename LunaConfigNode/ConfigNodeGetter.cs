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
            return ValueDict.GetSeveral(name);
        }

        /// <summary>
        /// Returns a value with the specified name if only 1 exists
        /// </summary>
        public MutableKeyValue<string, string> GetValue(string name)
        {
            return ValueDict.GetSingle(name);
        }

        /// <summary>
        /// Returns all the config nodes with the specified name
        /// </summary>
        public List<MutableKeyValue<string, ConfigNode>> GetNodes(string nodeName)
        {
            return NodeDict.GetSeveral(nodeName);
        }

        /// <summary>
        /// Returns the config nodes with the specified name if there's only 1
        /// </summary>
        public MutableKeyValue<string, ConfigNode> GetNode(string nodeName)
        {
            return NodeDict.GetSingle(nodeName);
        }
    }
}
