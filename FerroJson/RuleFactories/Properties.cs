using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Properties : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode node)
        {
            string propertyName;
            if (node.TryGetPropertyName(out propertyName))
            {
                return propertyName.Equals("properties");
            }

            return false;
        }

        public IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRule(ParseTreeNode node)
        {
            var rules = new Dictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>>();

            ParseTreeNode value = node.ChildNodes[1];
            var bootStrapper = BootstrapperLocator.Bootstrapper;

            foreach (var childNode in value.ChildNodes)
            {
                var validatorRuleFactories = bootStrapper.GetValidatorRuleFactoryLocator().Locate(childNode);
                foreach (var ruleFactory in validatorRuleFactories)
                {
                    var generatedRules = ruleFactory.GetValidatorRule(childNode);
                    foreach (var generatedRule in generatedRules)
                    {
                        if (rules.ContainsKey(generatedRule.Key))
                        {
                            rules[generatedRule.Key] = rules[generatedRule.Key].Concat(generatedRule.Value).ToList();
                        }
                        else
                        {
                            rules.Add(generatedRule.Key, generatedRule.Value);
                        }
                    }
                }
            }

            return rules;
        }
    }
}
