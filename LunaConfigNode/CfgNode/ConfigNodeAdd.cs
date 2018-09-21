namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Create the specified name and value
        /// </summary>
        public void CreateValue(string name, string value)
        {
            Values.Create(name, value);
        }

        /// <summary>
        /// Create or updates the given value
        /// </summary>
        public void CreateOrUpdateValue(string name, string value)
        {
            if (Values.Exists(name))
            {
                Values.Update(name, value);
            }
            else
            {
                Values.Create(name, value);
            }
        }

        /// <summary>
        /// Creates a new empty config node with the given name
        /// </summary>
        public ConfigNode CreateNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);

            Nodes.Create(name, newConfigNode);

            return newConfigNode;
        }

        /// <summary>
        /// Adds or updates the matching node names
        /// </summary>
        public void CreateOrReplaceNode(ConfigNode value)
        {
            if (Nodes.Exists(value.Name))
            {
                Nodes.Update(value.Name, value);
            }
            else
            {
                Nodes.Create(value.Name, value);
            }
        }
    }
}
