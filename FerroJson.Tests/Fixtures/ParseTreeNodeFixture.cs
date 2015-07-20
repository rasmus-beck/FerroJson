using System;
using System.Collections.Generic;
using System.IO;
using Irony.Parsing;
using Rhino.Mocks;

namespace FerroJson.Tests.Fixtures
{
    public static class ParseTreeNodeFixture
    {
		public static ParseTreeNode BuildPropertyNode(string key, object value)
        {
			// A property consists of a parent node with two child nodes. 
			// First child node is the property name and always a string.
			// Second child node is the property value and type depends on value
            var terminal = MockRepository.Mock<Terminal>("property");
            var nameNonTerminal = MockRepository.Mock<Terminal>("string");
            var valueNonTerminal = MockRepository.Mock<Terminal>(ConvertCSharpTypeToJsonType(value));
            var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "property");

			var propertyNameToken = MockRepository.Mock<Token>(nameNonTerminal, null, null, key);
			var propertyValueToken = MockRepository.Mock<Token>(valueNonTerminal, null, null, value);
            var propertyNameNode = MockRepository.Mock<ParseTreeNode>(propertyNameToken);
            var propertyValueNode = MockRepository.Mock<ParseTreeNode>(propertyValueToken);

            var propertyNode = MockRepository.Mock<ParseTreeNode>(propertyToken);
            propertyNode.ChildNodes.Add(propertyNameNode);
            propertyNode.ChildNodes.Add(propertyValueNode);

            return propertyNode;
        }

	    public static ParseTreeNode BuildObjectNode(ParseTreeNode propertyNode)
	    {
			var terminal = MockRepository.Mock<Terminal>("dummy");
			var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "dummy");
			var objectNode = MockRepository.Mock<ParseTreeNode>(propertyToken);

			objectNode.ChildNodes.Add(propertyNode);

			return objectNode;
	    }

        public static ParseTreeNode BuildObjectNode(IDictionary<string, object> properties)
        {
			var terminal = MockRepository.Mock<Terminal>("dummy");
			var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "dummy");
            var objectNode = MockRepository.Mock<ParseTreeNode>(propertyToken);

            foreach (var property in properties)
            {
				var propertyNode = BuildPropertyNode(property.Key, property.Value);
                objectNode.ChildNodes.Add(propertyNode);
            }

            return objectNode;
        }

	    private static string ConvertCSharpTypeToJsonType(object value)
	    {
		    if (null == value)
			    return "null";

		    switch (value.GetType().FullName)
		    {
				case "System.Boolean":
				    return "bool";
			    case "System.Byte":
				case "System.SByte":
				case "System.Int16":
				case "System.UInt16":
				case "System.Int32":
				case "System.UInt32":
				case "System.Int64":
				case "System.UInt64":
				    return "int";
				case "System.Single":
				case "System.Decimal":
				case "System.Double":
					return "decimal";
				case "System.String":
				case "System.Char":
				    return "string";
				case "System.Object":
				    return "object";
		    }

			throw new InvalidDataException("No type found that matches " + value.GetType().FullName);
	    }
    }
}
