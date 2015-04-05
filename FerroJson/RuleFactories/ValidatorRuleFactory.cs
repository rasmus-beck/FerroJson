using System;
using System.Collections.Generic;
using FerroJson.Bootstrapper;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public abstract class ValidatorRuleFactory : IValidatorRuleFactory
    {
        public abstract IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; protected set; }
        public abstract bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);

        public IValidatorRuleFactoryLocator ValidatorRuleFactoryLocator
        {
            get
            {
                var bootStrapper = BootstrapperLocator.Bootstrapper;
                return bootStrapper.GetValidatorRuleFactoryLocator();
            }
        }

        public bool IsType(ParseTreeNode jsonSchemaProperty, string requiredType)
        {
            var propertyDefinitionNode = GetPropertyDefinitionNode(jsonSchemaProperty);

            string foundType;
            if (propertyDefinitionNode.TryGetPropertyValueFromObject("type", out foundType))
            {
                return !String.IsNullOrEmpty(foundType) && foundType.ToLowerInvariant().Equals(requiredType);
            }
            return false;
        }

        public bool IsNamed(ParseTreeNode jsonSchemaProperty, string requiredPropertyName)
        {
            string propertyName;
            return jsonSchemaProperty.TryGetPropertyName(out propertyName) && propertyName.Equals(requiredPropertyName);
        }

        protected bool IsObjectNode(ParseTreeNode node)
        {
            if (IsValueObjectNode(node)) return true;

            var propertyDefinition = GetPropertyDefinitionNode(node);
            return IsValueObjectNode(propertyDefinition);            
        }

        private bool IsValueObjectNode(ParseTreeNode node)
        {
            string type;
            return null != node && node.TryGetPropertyValueFromObject("type", out type) && type.ToLowerInvariant().Equals("object");
        }

        public bool IsArrayNode(ParseTreeNode node)
        {
            var propertyDefinition = GetPropertyDefinitionNode(node);
            string type;
            return null != propertyDefinition && propertyDefinition.TryGetPropertyValueFromObject("type", out type) && type.ToLowerInvariant().Equals("array");
        }

        public bool HasProperty(ParseTreeNode jsonSchemaProperty, string requiredProperty)
        {
            var propertyDefinitioNode = GetPropertyDefinitionNode(jsonSchemaProperty);
            return propertyDefinitioNode.HasProperty(requiredProperty);
        }

        public ParseTreeNode GetPropertyDefinitionNode(ParseTreeNode jsonSchemaProperty)
        {
            if (jsonSchemaProperty.ChildNodes.Count != 2 || !jsonSchemaProperty.Term.Name.ToLowerInvariant().Equals("property"))
            {
                return null;
            }

            return jsonSchemaProperty.ChildNodes[1];
        }


		public virtual Func<ParseTreeNode, string> GetValidatorRules(string propertyName, ParseTreeNode valueNode)
		{
			throw new NotImplementedException("You must either implement GetValidatorRules(ParseTreeNode jsonSchemaProperty) or GetValidatorRules(string propertyName, ParseTreeNode valueNode) in your rule factory.");
		}
    }
}
