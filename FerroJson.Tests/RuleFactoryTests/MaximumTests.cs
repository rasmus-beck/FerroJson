using FerroJson.PropertyRuleFactories;
using Irony.Parsing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FerroJson.Tests.RuleFactoryTests
{
    [TestFixture]
    public class MaximumTests
    {
        [Test]
        public void MaximumProperty_Exists_CanCreateValidatorRule()
        {

            object value = 99;
            string name = "maximum";

            var objectNode = BuildObjectNode(name, value);

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);
            
            Assert.That(canCreate, Is.True);
        }

        private static ParseTreeNode BuildObjectNode(string name, object value)
        {
            var terminal = MockRepository.Mock<Terminal>("Property");

            var propertyNameToken = MockRepository.Mock<Token>(terminal, null, null, name);
            var propertyValueToken = MockRepository.Mock<Token>(terminal, null, null, value);
            var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "Property");
            var propertyNameNode = MockRepository.Mock<ParseTreeNode>(propertyNameToken);
            var propertyValueNode = MockRepository.Mock<ParseTreeNode>(propertyValueToken);

            var propertyNode = MockRepository.Mock<ParseTreeNode>(propertyToken);
            var objectNode = MockRepository.Mock<ParseTreeNode>(propertyToken);
            propertyNode.ChildNodes.Add(propertyNameNode);
            propertyNode.ChildNodes.Add(propertyValueNode);
            objectNode.ChildNodes.Add(propertyNode);
            return objectNode;
        }
    }
}
