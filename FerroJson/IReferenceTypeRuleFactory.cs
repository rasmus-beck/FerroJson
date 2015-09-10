using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FerroJson.Extensions;
using FerroJson.RuleFactories;

namespace FerroJson
{
	public interface IReferenceTypeRuleFactory
	{
		IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition);
		IDictionary<string, IProperty> GetValidatorRules(dynamic propertyDefinition);
	}

	public class ReferenceTypeRuleFactory : IReferenceTypeRuleFactory
	{
		private readonly IEnumerable<IValidatorRuleFactory> _validatorRuleFactories;

		public ReferenceTypeRuleFactory(IEnumerable<IValidatorRuleFactory> validatorRuleFactories)
		{
			_validatorRuleFactories = validatorRuleFactories;
		}

		public virtual IDictionary<string, IProperty> GetValidatorRules(dynamic propertyDefinition)
		{
			return GetValidatorRules(string.Empty, string.Empty, propertyDefinition);
		}

		public IDictionary<string, IProperty> GetValidatorRules(string nameSpace, string propertyName, dynamic propertyDefinition) 
		{
			IDictionary<string, IProperty> properties = GetValidatorRulesForThisProperty(nameSpace, propertyName, propertyDefinition);

			dynamic nestedPropertyDefinitions = (DynamicDictionary.DynamicDictionary)propertyDefinition.properties;
			if (null != nestedPropertyDefinitions)
			{
				nameSpace += propertyName;

				foreach (var name in nestedPropertyDefinitions.Keys)
				{
					dynamic value = (DynamicDictionary.DynamicDictionary)nestedPropertyDefinitions[name];

					if (value.type.HasValue && value.type.Value is string && value.type.Value.ToLowerInvariant() == "array")
					{
						if (!value.items.HasValue)
						{
							throw new InvalidDataException("Properties of type array must contain item definitions");
						}

						IDictionary<string, IProperty> nestedProperties = GetValidatorRules(nameSpace, name, value.items.Value);
						properties = properties.AddRange(nestedProperties);	
					}
					else
					{
						IDictionary<string, IProperty> nestedProperties = GetValidatorRules(nameSpace, name, value);
						properties = properties.AddRange(nestedProperties);
					}
				}
			}

			return properties;
		}

		//This name sucks bigtime... what happens here is more like an init or config or read defintion
		private IDictionary<string, IProperty> GetValidatorRulesForThisProperty(string nameSpace, string propertyName, dynamic propertyDefinition)
		{
			var properties = new Dictionary<string, IProperty>();

			if (String.IsNullOrEmpty(propertyName))
			{
				return properties;
			}

			var description = propertyDefinition.description.HasValue ? propertyDefinition.description.Value : null;

			var validatorRuleFactories = _validatorRuleFactories.Where(f => f.CanCreateValidatorRule(propertyDefinition));
			var objectRules = validatorRuleFactories.Select<IValidatorRuleFactory, Func<dynamic, string>>(ruleFactory => ruleFactory.GetValidatorRules(propertyDefinition));
			var objectProperty = new Property { Name = propertyName, Description = description, Rules = objectRules };
			properties.Add(nameSpace.AppendToNameSpace(propertyName), objectProperty);
			return properties;
		}
	}
}
