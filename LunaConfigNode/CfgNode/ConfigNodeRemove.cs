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
            Nodes.Remove(configNode.Name, configNode);
        }
    }
}
