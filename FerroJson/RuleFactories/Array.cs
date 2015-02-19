using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Array : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode node)
        {
            if (node == null || node.ChildNodes.Count != 2) return false;

            var propertyNode = node.ChildNodes[1];
            return IsArrayNode(propertyNode);
        }

        private bool IsArrayNode(ParseTreeNode node)
        {
            string type;
            return null != node && node.TryGetPropertyValueFromObject("type", out type) && type.ToLowerInvariant().Equals("array");
        }

        public IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRule(ParseTreeNode node)
        {
            var rules = new Dictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>>();
            var bootStrapper = BootstrapperLocator.Bootstrapper;

            string propertyName = node.GetPropertyName() + ".";
            var propertyNode = node.ChildNodes[1];

            var items = propertyNode.GetPropertyValueNodeFromObject("items");

            var validatorRuleFactories = bootStrapper.GetValidatorRuleFactoryLocator().Locate(items);
            foreach (var ruleFactory in validatorRuleFactories)
            {
                var generatedRules = ruleFactory.GetValidatorRule(items);
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

            return rules;
        }
    }
}