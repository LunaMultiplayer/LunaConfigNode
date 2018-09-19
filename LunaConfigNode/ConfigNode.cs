using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LunaConfigNode
{
    public partial class ConfigNode
    {
        private const string ValueSeparator = " = ";
        private const string OpenNodeSymbol = "{";
        private const string CloseNodeSymbol = "}";

        public string Name { get; set; } = string.Empty;
        public ConfigNode Parent { get; set; }

        private bool _useDictionaryForValues = true;
        private Dictionary<string, string> ValueDict { get; set; } = new Dictionary<string, string>();
        private List<KeyValuePair<string,string>> ValueList { get; set; }

        private bool _useDictionaryForNodes = true;
        private Dictionary<string, ConfigNode> NodeDict { get; set; } = new Dictionary<string, ConfigNode>();
        private List<ConfigNode> NodeList { get; set; }

        private int Depth => Parent?.Depth + 1 ?? 0;

        #region Constructor

        public ConfigNode()
        {
        }

        public ConfigNode(string contents)
        {
            var currentNode = this;
            using (var reader = new StringReader(contents))
            {
                var previousLine = string.Empty;
                string line;
                while ((line = reader.ReadLine()?.TrimStart()) != null)
                {
                    if (line.Contains(ValueSeparator))
                    {
                        currentNode.AddValue(line.Substring(0, line.IndexOf(ValueSeparator, StringComparison.Ordinal)).Trim(),
                            line.Substring(line.LastIndexOf(ValueSeparator, StringComparison.Ordinal) + ValueSeparator.Length).Trim());

                        continue;
                    }
                    if (line.Equals(OpenNodeSymbol))
                    {
                        currentNode = currentNode.AddNode(previousLine);
                        continue;
                    }
                    if (line.Equals(CloseNodeSymbol))
                    {
                        currentNode = currentNode.Parent;
                        continue;
                    }
                    previousLine = line;
                }
            }
        }

        internal ConfigNode(string name, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
        }

        #endregion
        
        #region Base overrides

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (Depth > 0) //When we are in a subnode initialize it
            {
                InitializeNode(builder);
            }

            if (_useDictionaryForValues)
            {
                foreach (var dictVal in ValueDict)
                {
                    GetFieldTabbing(builder);
                    builder.Append(dictVal.Key);
                    builder.Append(ValueSeparator);
                    builder.AppendLine(dictVal.Value);
                }
            }
            else
            {
                foreach (var keyVal in ValueList)
                {
                    GetFieldTabbing(builder);
                    builder.Append(keyVal.Key);
                    builder.Append(ValueSeparator);
                    builder.AppendLine(keyVal.Value);
                }
            }

            if (_useDictionaryForNodes)
            {
                foreach (var nodeDictValue in NodeDict.Values)
                {
                    builder.AppendLine(nodeDictValue.ToString());
                }
            }
            else
            {
                foreach (var node in NodeList)
                {
                    builder.AppendLine(node.ToString());
                }
            }
            if (Depth > 0)
            {
                FinishNode(builder);
            }
            return builder.ToString().TrimEnd();
        }

        #endregion

        #region Private helpers

        private void GetFieldTabbing(StringBuilder builder)
        {
            for (var i = 0; i < Depth; i++)
            {
                builder.Append('\t');
            }
        }

        private void GetNodeTabbing(StringBuilder builder)
        {
            for (var i = 0; i < Depth - 1; i++)
            {
                builder.Append('\t');
            }
        }

        private void InitializeNode(StringBuilder builder)
        {
            GetNodeTabbing(builder);
            builder.AppendLine(Name);
            GetNodeTabbing(builder);
            builder.AppendLine(OpenNodeSymbol);
        }

        private void FinishNode(StringBuilder builder)
        {
            GetNodeTabbing(builder);
            builder.Append(CloseNodeSymbol);
        }

        #endregion
    }
}
