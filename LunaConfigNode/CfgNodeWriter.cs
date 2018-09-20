using LunaConfigNode.CfgNode;
using System.Collections.Generic;
using System.Text;

namespace LunaConfigNode
{
    public static class CfgNodeWriter
    {
        public static string WriteConfigNode(ConfigNode node)
        {
            var builder = new StringBuilder();
            if (node.Depth > 0) //When we are in a subnode initialize it
            {
                InitializeNode(node.Name, node.Depth, builder);
            }

            WriteValues(node.Values.GetAll(), node.Depth, builder);
            WriteNodes(node.Nodes.GetAll(), builder);

            if (node.Depth > 0)
            {
                FinishNode(node.Depth, builder);
            }

            return builder.ToString().TrimEnd();
        }

        public static void WriteNodes(IEnumerable<CfgNodeValue<string, ConfigNode>> nodes, StringBuilder builder)
        {
            foreach (var keyVal in nodes)
            {
                builder.AppendLine(WriteConfigNode(keyVal.Value));
            }
        }

        public static void WriteValues(IEnumerable<CfgNodeValue<string, string>> values, int nodeDepth, StringBuilder builder)
        {
            foreach (var value in values)
            {
                GetFieldTabbing(nodeDepth, builder);
                builder.AppendLine(value.ToString());
            }
        }

        public static void GetFieldTabbing(int ownerNodeDepth, StringBuilder builder)
        {
            for (var i = 0; i < ownerNodeDepth; i++)
            {
                builder.Append('\t');
            }
        }

        public static void GetNodeTabbing(int nodeDepth, StringBuilder builder)
        {
            for (var i = 0; i < nodeDepth - 1; i++)
            {
                builder.Append('\t');
            }
        }

        public static void InitializeNode(string nodeName, int nodeDepth, StringBuilder builder)
        {
            GetNodeTabbing(nodeDepth, builder);
            builder.AppendLine(nodeName);
            GetNodeTabbing(nodeDepth, builder);
            builder.AppendLine(CfgNodeConstants.OpenNodeSymbol);
        }

        public static void FinishNode(int nodeDepth, StringBuilder builder)
        {
            GetNodeTabbing(nodeDepth, builder);
            builder.Append(CfgNodeConstants.CloseNodeSymbol);
        }
    }
}
