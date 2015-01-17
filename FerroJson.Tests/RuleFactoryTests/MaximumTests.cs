using System.Collections;
using System.Collections.Generic;
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

            var objectNode = BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);
            
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_ExistsWithOthers_CanCreateValidatorRule()
        {

            object value = 99;
            string name = "maximum";

            var objectNode = BuildObjectNode(new Dictionary<string, object> { { name, value }, { "dummy", "dummy" } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_DoesNotExist_CannotCreateValidatorRule()
        {

            object value = 99;
            string name = "dummy";

            var objectNode = BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.False);
        }

        private static ParseTreeNode BuildObjectNode(IDictionary<string, object> properties)
        {
            var terminal = MockRepository.Mock<Terminal>("dummy");
            var propertyToken = MockRepository.Mock<Token>(terminal, null, null, "dummy");
            var objectNode = MockRepository.Mock<ParseTreeNode>(propertyToken);

            foreach (var property in properties)
            {
                var propertyNameToken = MockRepository.Mock<Token>(terminal, null, null, property.Key);
                var propertyValueToken = MockRepository.Mock<Token>(terminal, null, null, property.Value);
                var propertyNameNode = MockRepository.Mock<ParseTreeNode>(propertyNameToken);
                var propertyValueNode = MockRepository.Mock<ParseTreeNode>(propertyValueToken);

                var propertyNode = MockRepository.Mock<ParseTreeNode>(propertyToken);
                propertyNode.ChildNodes.Add(propertyNameNode);
                propertyNode.ChildNodes.Add(propertyValueNode);

                objectNode.ChildNodes.Add(propertyNode);
            }

            return objectNode;
        }
    }
}
