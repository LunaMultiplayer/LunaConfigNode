using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LunaConfigNode
{
    class VesselImplementation
    {
        public VesselImplementation(ConfigNode node)
        {
            var parts = new Dictionary<string, ConfigNode>();

            foreach (var partNode in node.GetNodes("PART"))
            {
                if (partNode.TryGetValue("uid", out var value))
                {
                    parts.Add(value, partNode);
                }
            }
        }
    }
}
