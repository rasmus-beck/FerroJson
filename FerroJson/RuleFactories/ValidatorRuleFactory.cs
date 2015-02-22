using System;
using System.Collections.Generic;
using System.Linq;
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

        protected IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetRulesFromObject(ParseTreeNode node, string propertyName)
        {
            var nodeList = new ParseTreeNodeList {node};
            return GetRulesFromObject(nodeList, propertyName);
        }

        protected IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetRulesFromObject(ParseTreeNodeList nodeList, string propertyName)
        {
            var rules = new Dictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>>();

            if (!String.IsNullOrEmpty(propertyName))
                propertyName += ".";

            foreach (var node in nodeList)
            {
                var validatorRuleFactories = ValidatorRuleFactoryLocator.Locate(node);

                foreach (var ruleFactory in validatorRuleFactories)
                {
                    var generatedRules = ruleFactory.GetValidatorRules(node);
                    foreach (var generatedRule in generatedRules)
                    {
                        var key = propertyName + generatedRule.Key;
                        if (rules.ContainsKey(key))
                        {
                            rules[key] = rules[key].Concat(generatedRule.Value).ToList();
                        }
                        else
                        {
                            rules.Add(key, generatedRule.Value);
                        }
                    }
                }
            }

            return rules;
        }

        public virtual IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(ParseTreeNode jsonSchemaProperty)
        {
            var propertyDefinitionNode = GetPropertyDefinitionNode(jsonSchemaProperty);
            var propertyName = jsonSchemaProperty.GetPropertyName();
            return GetValidatorRules(propertyName, propertyDefinitionNode);
        }

        public virtual IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(string propertyName, ParseTreeNode valueNode)
        {
            throw new NotImplementedException("You must either implement GetValidatorRules(ParseTreeNode jsonSchemaProperty) or GetValidatorRules(string propertyName, ParseTreeNode valueNode) in your rule factory.");
        }
    }
}
