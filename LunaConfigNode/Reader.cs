using System;
using System.IO;

namespace LunaConfigNode
{
    internal static class Reader
    {
        internal static ConfigNode ReadFromContent(string content)
        {
            var currentNode = new ConfigNode();
            using (var reader = new StringReader(content))
            {
                var previousLine = string.Empty;
                string line;
                while ((line = reader.ReadLine()?.Trim()) != null)
                {
                    if (line.Contains(" = "))
                    {
                        currentNode.AddValue(line.Substring(0, line.IndexOf(" ", StringComparison.Ordinal)).Trim(), 
                            line.Substring(line.LastIndexOf(" ", StringComparison.Ordinal)).Trim());

                        continue;
                    }
                    if (line.Equals("{"))
                    {
                        currentNode = currentNode.AddNode(previousLine);
                        continue;
                    }
                    if (line.Equals("}"))
                    {
                        currentNode = currentNode.Parent;
                        continue;
                    }
                    previousLine = line;
                }
            }

            return currentNode;
        }

        internal static ConfigNode ReadFromFile(string path)
        {
            if (File.Exists(path))
            {
                return ReadFromContent(File.ReadAllText(path));
            }

            throw new FileNotFoundException($"File: {path} not found");
        }
    }
}
