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
                    if (line.Contains(" ="))
                    {
                        var splitVal = line.Split('=');
                        currentNode.AddValue(splitVal[0].Trim(), splitVal.Length > 1 ? splitVal[1].Trim() : string.Empty);
                        continue;
                    }
                    if (line.Contains("{") && line.Length == 1)
                    {
                        currentNode = currentNode.AddNode(previousLine);
                        continue;
                    }
                    if (line.Contains("}") && line.Length == 1)
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
