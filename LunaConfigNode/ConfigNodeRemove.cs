namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Removes the specified value from this config node
        /// </summary>
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

        /// <summary>
        /// Removes the given config node
        /// </summary>
        public void RemoveNode(ConfigNode configNode)
        {
            if (_useDictionaryForNodes)
            {
                NodeDict.Remove(configNode.Name);
            }
            else
            {
                NodeList.Remove(configNode);
            }
        }
    }
}
