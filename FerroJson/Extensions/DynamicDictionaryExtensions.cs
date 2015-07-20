using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.Extensions
{
	public static class DynamicDictionaryExtensions
	{
		public static DynamicDictionary.DynamicDictionary AsDynamicDictionary(this ParseTree parseTree)
		{
			return AsDynamicDictionary(parseTree.Root);
		}

		public static DynamicDictionary.DynamicDictionary AsDynamicDictionary(this ParseTreeNode node)
		{
			var list = node.ChildNodes.Select(childNode => PropertyAsDynamicDictionaryKeyValuePair(childNode)).ToList();
			return DynamicDictionary.DynamicDictionary.Create(list);
		}

		private static KeyValuePair<string, dynamic> PropertyAsDynamicDictionaryKeyValuePair(ParseTreeNode node)
		{
			var propertyName = node.GetPropertyName();
			var valueNode = node.ChildNodes.Skip(1).Take(1).FirstOrDefault();
			var value = GetValue(valueNode);
			return new KeyValuePair<string, dynamic>(propertyName, value);
		}

		private static dynamic GetValue(ParseTreeNode node)
		{
			var term = node.Term.Name.ToLowerInvariant();
			dynamic value = null;
			switch (term)
			{
				case "object":
					value = AsDynamicDictionary(node);
					break;
				case "array":
					value = ArrayAsDynamicDictionary(node);
					break;
				case "string":
					value = node.Token.ValueString;
					break;
				case "int":
					value = Convert.ToInt32(node.Token.Value);
					break;
				case "decimal":
					value = Convert.ToDecimal(node.Token.Value);
					break;
				case "bool":
					value = Convert.ToBoolean(node.Token.Value);
					break;
				case "null":
					value = null;
					break;
			}

			return value;
		}

		private static IEnumerable<dynamic> ArrayAsDynamicDictionary(ParseTreeNode node)
		{
			return node.ChildNodes.Select(childNode => GetValue(childNode)).ToList();
		}
	}
}
