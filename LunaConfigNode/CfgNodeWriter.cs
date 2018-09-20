using LunaConfigNode.CfgNode;
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
                InitializeNode(node, builder);
            }

            foreach (var value in node.Values.GetAll())
            {
                GetFieldTabbing(node, builder);
                builder.AppendLine(value.ToString());
            }

            foreach (var value in node.Nodes.GetAllValues())
            {
                builder.AppendLine(value.ToString());
            }

            if (node.Depth > 0)
            {
                FinishNode(node, builder);
            }

            return builder.ToString().TrimEnd();
        }

        private static void GetFieldTabbing(ConfigNode node, StringBuilder builder)
        {
            for (var i = 0; i < node.Depth; i++)
            {
                builder.Append('\t');
            }
        }

        private static void GetNodeTabbing(ConfigNode node, StringBuilder builder)
        {
            for (var i = 0; i < node.Depth - 1; i++)
            {
                builder.Append('\t');
            }
        }

        private static void InitializeNode(ConfigNode node, StringBuilder builder)
        {
            GetNodeTabbing(node, builder);
            builder.AppendLine(node.Name);
            GetNodeTabbing(node, builder);
            builder.AppendLine(CfgNodeConstants.OpenNodeSymbol);
        }

        private static void FinishNode(ConfigNode node, StringBuilder builder)
        {
            GetNodeTabbing(node, builder);
            builder.Append(CfgNodeConstants.CloseNodeSymbol);
        }

    }
}
