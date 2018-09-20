using System.Collections.Generic;

namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Returns all the values inside this ConfigNode
        /// </summary>
        public List<CfgNodeValue<string, string>> GetAllValues()
        {
            return Values.GetAll();
        }

        /// <summary>
        /// Returns all the nodes inside this ConfigNode
        /// </summary>
        public List<ConfigNode> GetAllNodes()
        {
            return Nodes.GetAllValues();
        }

        /// <summary>
        /// Returns all the values with the specified name
        /// </summary>
        public List<CfgNodeValue<string, string>> GetValues(string name)
        {
            return Values.GetSeveral(name);
        }

        /// <summary>
        /// Returns a value with the specified name if only 1 exists
        /// </summary>
        public CfgNodeValue<string, string> GetValue(string name)
        {
            return Values.GetSingle(name);
        }

        /// <summary>
        /// Returns all the config nodes with the specified name
        /// </summary>
        public List<CfgNodeValue<string, ConfigNode>> GetNodes(string nodeName)
        {
            return Nodes.GetSeveral(nodeName);
        }

        /// <summary>
        /// Returns the config nodes with the specified name if there's only 1
        /// </summary>
        public CfgNodeValue<string, ConfigNode> GetNode(string nodeName)
        {
            return Nodes.GetSingle(nodeName);
        }
    }
}
