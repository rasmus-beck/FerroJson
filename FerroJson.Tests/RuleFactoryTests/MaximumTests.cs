using System.Collections.Generic;
using FerroJson.PropertyRuleFactories;
using FerroJson.Tests.Fixtures;
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

            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);
            
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_ExistsWithOthers_CanCreateValidatorRule()
        {

            object value = 99;
            string name = "maximum";

            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value }, { "dummy", "dummy" } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_DoesNotExist_CannotCreateValidatorRule()
        {

            object value = 99;
            string name = "dummy";

            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.False);
        }

        [Test]
        public void MaximumProperty_Exists_Validates()
        {
            object value = 99;
            string name = "maximum";

            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 50);
            var result = rule.Invoke(property);

            Assert.That(result, Is.True);
        }

        [Test]
        public void MaximumProperty_Exists_DoesNotValidate()
        {
            object value = 99;
            string name = "maximum";

            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 500);
            var result = rule.Invoke(property);

            Assert.That(result, Is.False);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_Validates()
        {
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "maximum", 99 }, { "exclusiveMaximum", true } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 98);
            var result = rule.Invoke(property);

            Assert.That(result, Is.True);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_DoesNotValidate()
        {
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "maximum", 99 }, { "exclusiveMaximum", true } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rule.Invoke(property);

            Assert.That(result, Is.False);
        }

        
    }
}
