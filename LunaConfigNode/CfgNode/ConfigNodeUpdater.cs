﻿namespace LunaConfigNode.CfgNode
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
        /// Replaces the nodes with the matching node name and configNode if it exists for the one given as parameter
        /// </summary>
        public void ReplaceNode(ConfigNode oldNode, ConfigNode updatedNode)
        {
            updatedNode.Parent = this;
            Nodes.Replace(new CfgNodeValue<string, ConfigNode>(oldNode.Name, oldNode), new CfgNodeValue<string, ConfigNode>(updatedNode.Name, updatedNode));
        }
    }
}
