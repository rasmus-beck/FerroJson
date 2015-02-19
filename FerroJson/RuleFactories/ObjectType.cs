using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class ObjectType : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode node)
        {
            if (IsObjectNode(node)) return true;

            if (node.ChildNodes.Count != 2) return false;

            ParseTreeNode propertyNode = node.ChildNodes[1];
            return IsObjectNode(propertyNode);
        }

        private bool IsObjectNode(ParseTreeNode node)
        {
            string type;
            return null != node && node.TryGetPropertyValueFromObject("type", out type) && type.ToLowerInvariant().Equals("object");
        }

        public IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRule(ParseTreeNode node)
        {
            var bootStrapper = BootstrapperLocator.Bootstrapper;
            //First get name of the property - will be null if we are dealing with the root node
            string propertyName;
            var objectNode = node;
            if (node.TryGetPropertyName(out propertyName))
            {
                propertyName += ".";
                objectNode = node.ChildNodes[1];
            }

            var rules = new Dictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>>();

            //Second go through each property
            foreach (var childNode in objectNode.ChildNodes)
            {
                var validatorRuleFactories = bootStrapper.GetValidatorRuleFactoryLocator().Locate(childNode);
                foreach (var ruleFactory in validatorRuleFactories)
                {
                    var generatedRules = ruleFactory.GetValidatorRule(childNode);
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
    }
}