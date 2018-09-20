namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Removes the specified value from this config node
        /// </summary>
        public void RemoveValue(string name)
        {
            ValueDict.Remove(name);
        }

        /// <summary>
        /// Removes the given config node
        /// </summary>
        public void RemoveNode(ConfigNode configNode)
        {
            NodeDict.Remove(configNode.Name, configNode);
        }
    }
}
