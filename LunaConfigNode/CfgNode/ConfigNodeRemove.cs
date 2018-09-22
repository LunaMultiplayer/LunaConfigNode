namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Removes the specified value from this config node
        /// </summary>
        public void RemoveValue(string name)
        {
            Values.Remove(name);
        }

        /// <summary>
        /// Removes the given config node
        /// </summary>
        public void RemoveNode(ConfigNode configNode)
        {
            Nodes.Remove(new CfgNodeValue<string, ConfigNode>(configNode.Name, configNode));
        }

        /// <summary>
        /// Removes ALL the nodes that have the given name
        /// </summary>
        public void RemoveNode(string name)
        {
            Nodes.Remove(name);
        }
    }
}
