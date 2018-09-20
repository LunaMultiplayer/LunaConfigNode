namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Updates the given value if it exists
        /// </summary>
        public void UpdateValue(string name, string value)
        {
            ValueDict.Update(name, value);
        }

        /// <summary>
        /// Updates all the nodes with the matching node name if they exist
        /// </summary>
        public void UpdateNode(ConfigNode updatedNode)
        {
            NodeDict.Update(updatedNode.Name, updatedNode);
        }
    }
}
