using System.ComponentModel;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.Extensions
{
    public static class ParseTreeNodeExtensions
    {
        public static bool HasProperty(this ParseTreeNode node, string propertyName)
        {
            return node.ChildNodes.SelectMany(x => x.ChildNodes).Any(y => null != y.Token && y.Token.ValueString == propertyName);
        }

        public static T GetPropertyValueFromObject<T>(this ParseTreeNode node, string propertyName)
        {
            var propertyNode = node.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token != null && y.Token.ValueString == propertyName));
            return GetValue<T>(propertyNode);
        }

        public static bool TryGetPropertyValueFromObject<T>(this ParseTreeNode node, string propertyName, out T value)
        {
            var propertyNode = node.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token != null && y.Token.ValueString == propertyName));
            return TryGetValue(propertyNode, out value);
        }

        public static bool TryGetValue<T>(this ParseTreeNode node, out T value)
        {
            if (null == node || 2 != node.ChildNodes.Count)
            {
                value = default(T);
                return false;
            }

            try
            {
                value = GetValue<T>(node);
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public static bool TryGetPropertyName(this ParseTreeNode node, out string value)
        {
            if (null == node || 2 != node.ChildNodes.Count)
            {
                value = null;
                return false;
            }

            try
            {
                value = GetPropertyName(node);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }
        
        public static T GetValue<T>(this ParseTreeNode node)
        {
            var value = node.ChildNodes[1].Token.ValueString;
            var typeConverter = TypeDescriptor.GetConverter(typeof(T));
            return (T)typeConverter.ConvertFromString(value);
        }

        public static string GetPropertyName(this ParseTreeNode node)
        {
            return node.ChildNodes[0].Token.ValueString;
        }
    }
}
