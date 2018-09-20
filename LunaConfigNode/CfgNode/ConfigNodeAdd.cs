namespace LunaConfigNode.CfgNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Adds the specified name and value
        /// </summary>
        public void AddValue(string name, string value)
        {
            Values.Create(name, value);
        }

        /// <summary>
        /// Adds or updates the given value
        /// </summary>
        public void AddOrUpdateValue(string name, string value)
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
        /// Adds a new config node with the given name
        /// </summary>
        public ConfigNode AddNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);

            Nodes.Create(name, newConfigNode);

            return newConfigNode;
        }

        /// <summary>
        /// Adds or updates the matching node names
        /// </summary>
        public void AddOrUpdateNode(ConfigNode value)
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
