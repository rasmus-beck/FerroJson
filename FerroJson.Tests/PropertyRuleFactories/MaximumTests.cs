using System.Collections.Generic;
using FerroJson.Extensions;
using FerroJson.RuleFactories;
using FerroJson.Tests.Fixtures;
using NUnit.Framework;

namespace FerroJson.Tests.PropertyRuleFactories
{
    [TestFixture]
    public class MaximumTests
    {
        [Test]
        public void MaximumProperty_Exists_CanCreateValidatorRule()
        {
            //Given
            var value = 99;
            const string name = "maximum";
            //var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { {"type", "int"}, { name, value } });
            var propertyNode = ParseTreeNodeFixture.BuildPropertyNode(name, value);
	        var objectNode = ParseTreeNodeFixture.BuildObjectNode(propertyNode);
	        dynamic dict = objectNode.AsDynamicDictionary();


            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);
            
            //Then
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_ExistsWithOthers_CanCreateValidatorRule()
        {
            //Given
            var value = 99;
            const string name = "maximum";
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { name, value }, { "dummy", "dummy" } });
			dynamic dict = objectNode.AsDynamicDictionary();

            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            //Then
            Assert.That(canCreate, Is.True);
        }

        [Test]
        public void MaximumProperty_DoesNotExist_CannotCreateValidatorRule()
        {
            //Given
            var value = 99;
            const string name = "dummy";
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { name, value } });
			dynamic dict = objectNode.AsDynamicDictionary();

            //When
            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            //Then
            Assert.That(canCreate, Is.False);
        }

        [Test]
        public void MaximumProperty_Exists_Validates()
        {
            //Given
            var value = 99;
            const string name = "maximum";
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { name, value } });
			dynamic dict = objectNode.AsDynamicDictionary();

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);
            
            //When
			//var property = ParseTreeNodeFixture.BuildPropertyNode("age", 50);
            var result = rules.Invoke(50);

            //Then
            Assert.That(result, Is.Null);
        }

        [Test]
        public void MaximumProperty_Exists_DoesNotValidate()
        {
            //Given
            var value = 99;
            const string name = "maximum";
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { name, value } });
			dynamic dict = objectNode.AsDynamicDictionary();

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);
            
            //When
			var property = ParseTreeNodeFixture.BuildPropertyNode("age", 500);
            var result = rules.Invoke(property);

            //Then
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void MaximumProperty_ExactValue_Validates()
        {
            //Given
            var value = 99;
            const string name = "maximum";
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { name, value } });
			dynamic dict = objectNode.AsDynamicDictionary();

            var maximumRuleFactory = new Maximum();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);
            
            //When
			//var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rules.Invoke(99);

            //Then
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_Validates()
        {
            //Given
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
			dynamic dict = objectNode.AsDynamicDictionary();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);

            //When
			//var property = ParseTreeNodeFixture.BuildPropertyNode("age", 98);
            var result = rules.Invoke(98);

            //Then
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ExclusiveMaximumProperty_ExactValue_DoesNotValidate()
        {
            //Given
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
			dynamic dict = objectNode.AsDynamicDictionary();
            var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);

            //When
			var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rules.Invoke(property);

            //Then
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void ExclusiveMaximumProperty_Exists_DoesNotValidate()
        {
            //Given
			var objectNode = ParseTreeNodeFixture.BuildObjectNode(new Dictionary<string, object> { { "type", "number" }, { "maximum", 99 }, { "exclusiveMaximum", true } });
            var maximumRuleFactory = new Maximum();
			dynamic dict = objectNode.AsDynamicDictionary();
			var canCreate = maximumRuleFactory.CanCreateValidatorRule(dict);

            Assert.That(canCreate, Is.True);

            var rules = maximumRuleFactory.GetValidatorRules(dict);

            Assert.That(rules, Is.Not.Null);

            //When
			var property = ParseTreeNodeFixture.BuildPropertyNode("age", 99);
            var result = rules.Invoke(property);

            //Then
            Assert.That(result, Is.Not.Null);
        }
    }
}
