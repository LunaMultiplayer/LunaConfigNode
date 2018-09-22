namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Adds the specified name-value
        /// </summary>
        public void CreateValue(CfgNodeValue<string,string> value)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Adds the given node as a child
        /// </summary>
        public void AddNode(ConfigNode value)
        {
            value.Parent = this;
            Nodes.Add(new CfgNodeValue<string, ConfigNode>(value.Name, value));
        }
    }
}
