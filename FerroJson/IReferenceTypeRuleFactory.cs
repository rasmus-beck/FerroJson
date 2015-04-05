using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using FerroJson.Extensions;
using FerroJson.RuleFactories;
using Irony.Parsing;

namespace FerroJson
{
	public interface IReferenceTypeRuleFactory
	{
		bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
		IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, string propertyDescription, ParseTreeNode value);
	}

	public abstract class ReferenceTypeRuleFactory : IReferenceTypeRuleFactory
	{
		protected IEnumerable<IValidatorRuleFactory> ValidatorRuleFactories;

		protected ReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories)
		{
			ValidatorRuleFactories = validatorRuleFactories;
		}

		public abstract bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
		public abstract IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, string propertyDescription, ParseTreeNode value);

		protected IDictionary<string, IProperty> GetOwnProperty(string nameSpace, string propertyName, string propertyDescription, ParseTreeNode jsonSchemaProperty)
		{
			if (String.IsNullOrEmpty(propertyName))
				return null;

			var properties = new Dictionary<string, IProperty>();

			if (!String.IsNullOrEmpty(nameSpace))
			{
				nameSpace += ".";
			}

			var validatorRuleFactories = ValidatorRuleFactories.Where(f => f.CanCreateValidatorRule(jsonSchemaProperty)).ToList();
			var objectRules = validatorRuleFactories.Select(ruleFactory => ruleFactory.GetValidatorRules(nameSpace, jsonSchemaProperty)).ToList();
			var objectProperty = new Property { Name = propertyName, Description = propertyDescription, Rules = objectRules };
			properties.Add(nameSpace + propertyName, objectProperty);
			return properties;
		}
	}

	public class PropertyReferenceTypeRuleFactory : ReferenceTypeRuleFactory
	{
		public PropertyReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories) : base(validatorRuleFactories)
		{
		}

		public override bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
		{
			return jsonSchemaProperty.IsPropertyNode();
		}

		public override IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, string propertyDescription,
			ParseTreeNode value)
		{
			return GetOwnProperty(nameSpace, propertyName, propertyDescription, value);
		}
	}

	public class ObjectReferenceTypeRuleFactory : ReferenceTypeRuleFactory
	{
		public ObjectReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories) : base(validatorRuleFactories)
		{
		}

		public override bool CanCreateValidatorRule(ParseTreeNode jsonSchemaObject)
		{
			return jsonSchemaObject.IsObjectNode();
		}

		public override IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, string propertyDescription, ParseTreeNode value) 
		{
			var properties = GetOwnProperty(nameSpace, propertyName, propertyDescription, value);

			var nestedPropertyDefinitions = value.GetPropertyValueNodeFromObject("properties");
			if (null != nestedPropertyDefinitions)
			{
				nameSpace += propertyName;
				var locator = BootstrapperLocator.Bootstrapper.GetReferenceTypeRuleFactoryLocator();

				foreach (var propertyDefinition in nestedPropertyDefinitions.ChildNodes)
				{
					var validator = locator.Locate(propertyDefinition);
					if (null != validator)
					{
						var nPropertyName = propertyDefinition.GetPropertyName();
						var nPropertyDescription = propertyDefinition.GetPropertyDescription();
						var nPropertyValueNode = propertyDefinition.GetPropertyValueNode();
						var nestedProperties = validator.GetValidatorRules(nameSpace, nPropertyName, nPropertyDescription, nPropertyValueNode);
						properties = properties.AddRange(nestedProperties);
					}
				}
			}

			return properties;
		}
	}
}
