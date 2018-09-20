namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        /// <summary>
        /// Adds the specified name and value
        /// </summary>
        public void AddValue(string name, string value)
        {
            ValueDict.Create(name, value);
        }

        /// <summary>
        /// Adds or updates the given value
        /// </summary>
        public void AddOrUpdateValue(string name, string value)
        {
            if (ValueDict.Exists(name))
            {
                ValueDict.Update(name, value);
            }
            else
            {
                ValueDict.Create(name, value);
            }
        }

        /// <summary>
        /// Adds a new config node with the given name
        /// </summary>
        public ConfigNode AddNode(string name)
        {
            var newConfigNode = new ConfigNode(name, this);

            NodeDict.Create(name, newConfigNode);

            return newConfigNode;
        }

        /// <summary>
        /// Adds or updates the matching node names
        /// </summary>
        public void AddOrUpdateNode(ConfigNode value)
        {
            if (NodeDict.Exists(value.Name))
            {
                NodeDict.Update(value.Name, value);
            }
            else
            {
                NodeDict.Create(value.Name, value);
            }
        }
    }
}
