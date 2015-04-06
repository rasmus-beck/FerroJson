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
		bool CanCreateValidatorRule(dynamic propertyDefinition);
		IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition);
		IDictionary<string, IProperty> GetValidatorRules(dynamic propertyDefinition);
	}

	public abstract class ReferenceTypeRuleFactory : IReferenceTypeRuleFactory
	{
		protected IEnumerable<IValidatorRuleFactory> ValidatorRuleFactories;

		protected ReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories)
		{
			ValidatorRuleFactories = validatorRuleFactories;
		}

		public abstract bool CanCreateValidatorRule(dynamic propertyDefinition);
		public abstract IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition);

		public virtual IDictionary<string, IProperty> GetValidatorRules(dynamic propertyDefinition)
		{
			return GetValidatorRules(string.Empty, string.Empty, propertyDefinition);
		}

		protected IDictionary<string, IProperty> GetOwnProperty(string nameSpace, string propertyName, dynamic propertyDefinition)
		{
			var properties = new Dictionary<string, IProperty>();

			if (String.IsNullOrEmpty(propertyName))
				return properties;

			if (!String.IsNullOrEmpty(nameSpace))
			{
				nameSpace += ".";
			}

			var description = propertyDefinition.description.HasValue ? propertyDefinition.description.Value : null;

			var validatorRuleFactories = ValidatorRuleFactories.Where(f => f.CanCreateValidatorRule(propertyDefinition)).ToList();
			var objectRules = validatorRuleFactories.Select<IValidatorRuleFactory, Func<ParseTreeNode, string>>(ruleFactory => ruleFactory.GetValidatorRules(propertyDefinition)).ToList();
			var objectProperty = new Property { Name = propertyName, Description = description, Rules = objectRules };
			properties.Add(nameSpace + propertyName, objectProperty);
			return properties;
		}
	}

	public class PropertyReferenceTypeRuleFactory : ReferenceTypeRuleFactory
	{
		public PropertyReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories) : base(validatorRuleFactories)
		{
		}

		public override bool CanCreateValidatorRule(dynamic propertyDefinition)
		{
			return propertyDefinition is KeyValuePair<string, dynamic>;
		}

		public override IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition)
		{
			return GetOwnProperty(nameSpace, propertyName, propertyDefinition);
		}
	}

	public class ObjectReferenceTypeRuleFactory : ReferenceTypeRuleFactory
	{
		private IReferenceTypeRuleFactoryLocator _locator;

		public ObjectReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories) : base(validatorRuleFactories)
		{
			_locator = BootstrapperLocator.Bootstrapper.GetReferenceTypeRuleFactoryLocator();
		}

		public override bool CanCreateValidatorRule(dynamic propertyDefinition)
		{
			return propertyDefinition is DynamicDictionary.DynamicDictionary;
		}

		public override IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition) 
		{
			IDictionary<string, IProperty> properties = GetOwnProperty(nameSpace, propertyName, propertyDefinition);

			dynamic nestedPropertyDefinitions = (DynamicDictionary.DynamicDictionary)propertyDefinition.properties;
			if (null != nestedPropertyDefinitions)
			{
				nameSpace += propertyName;

				foreach (var name in nestedPropertyDefinitions.Keys)
				{
					dynamic value = (DynamicDictionary.DynamicDictionary)nestedPropertyDefinitions[name];
					var keyValuePair = new KeyValuePair<string, dynamic>(name, value);
					var validator = _locator.Locate(keyValuePair);
					if (null != validator)
					{
						IDictionary<string, IProperty> nestedProperties = validator.GetValidatorRules(nameSpace, name, value);
						properties = properties.AddRange(nestedProperties);
					}
				}
			}

			return properties;
		}
	}
}
