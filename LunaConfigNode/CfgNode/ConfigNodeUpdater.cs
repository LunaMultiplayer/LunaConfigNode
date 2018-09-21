namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Updates the given value if it exists
        /// </summary>
        public void UpdateValue(string name, string value)
        {
            Values.Update(name, value);
        }

        /// <summary>
        /// Replaces all the nodes with the matching node name if they exist for the one given as parameter
        /// </summary>
        public void ReplaceNode(ConfigNode updatedNode)
        {
            Nodes.Update(updatedNode.Name, updatedNode);
        }
    }
}
