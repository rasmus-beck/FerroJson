using System.Collections.Generic;
using FerroJson.PropertyRuleFactories;
using FerroJson.Tests.Fixtures;
using NUnit.Framework;

namespace FerroJson.Tests.RuleFactoryTests
{
    [TestFixture]
    public class MaximumTests
    {
        [Test]
        public void MaximumProperty_Exists_CanCreateValidatorRule()
        {
            //Given
            object value = 99;
            const string name = "maximum";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);
            
            //Then
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_ExistsWithOthers_CanCreateValidatorRule()
        {
            //Given
            object value = 99;
            const string name = "maximum";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value }, { "dummy", "dummy" } });

            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            //Then
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_DoesNotExist_CannotCreateValidatorRule()
        {
            //Given
            object value = 99;
            const string name = "dummy";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            //Then
            Assert.That(canCreate, Is.False);
        }

        [Test]
        public void MaximumProperty_Exists_Validates()
        {
            //Given
            object value = 99;
            const string name = "maximum";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            
            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 50);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.True);
        }

        [Test]
        public void MaximumProperty_Exists_DoesNotValidate()
        {
            //Given
            object value = 99;
            const string name = "maximum";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            
            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 500);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.False);
        }

        [Test]
        public void MaximumProperty_ExactValue_Validates()
        {
            //Given
            object value = 99;
            const string name = "maximum";
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { name, value } });

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);
            
            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.True);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_Validates()
        {
            //Given
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);

            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 98);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.True);
        }

        [Test]
        public void ExclusiveMaximumProperty_ExactValue_DoesNotValidate()
        {
            //Given
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);

            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.False);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_DoesNotValidate()
        {
            //Given
            var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(objectNode);

            Assert.That(canCreate, Is.True);

            var rule = maximumRuleFactory.GetValidatorRule(objectNode);

            Assert.That(rule, Is.Not.Null);

            //When
            var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rule.Invoke(property);

            //Then
            Assert.That(result, Is.False);
        }
    }
}
