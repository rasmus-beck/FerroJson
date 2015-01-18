using System.Collections.Generic;
using Irony.Parsing;
using Rhino.Mocks;

namespace FerroJson.Tests.Fixtures
{
    public static class ParseTreeNodeFixture
    {
        public static ParseTreeNode BuildPropertyNode(string key, object value)
        {
            var terminal = MockRepository.Mock<Terminal>("dummy");
            var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "dummy");

            var propertyNameToken = MockRepository.Mock<Token>(terminal, null, null, key);
            var propertyValueToken = MockRepository.Mock<Token>(terminal, null, null, value);
            var propertyNameNode = MockRepository.Mock<ParseTreeNode>(propertyNameToken);
            var propertyValueNode = MockRepository.Mock<ParseTreeNode>(propertyValueToken);

            var propertyNode = MockRepository.Mock<ParseTreeNode>(propertyToken);
            propertyNode.ChildNodes.Add(propertyNameNode);
            propertyNode.ChildNodes.Add(propertyValueNode);

            return propertyNode;
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
    }
}
