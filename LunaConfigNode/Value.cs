using System.Globalization;
using System.Text;

namespace LunaConfigNode
{
    public class Value
    {
        public string Name { get; set; }
        public string Val { get; private set; } = string.Empty;
        internal ConfigNode Parent { get; set; }

        #region Constructors

        internal Value(string name, dynamic val, ConfigNode parent)
        {
            Name = name;
            Parent = parent;
            SetValue(val);
        }

        #endregion

        #region Public methods

        public void SetValue(dynamic value)
        {
            Val = value.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

        #region Base overrides

        public override string ToString()
        {
            var builder = new StringBuilder();
            for (var i = 0; i < Parent.Depth; i++)
            {
                builder.Append('\t');
            }
            builder.Append(Name).Append(" = ").Append(Val);
            return builder.ToString();
        }

        #endregion
    }
}
