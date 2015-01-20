using System;
using System.Collections.Generic;
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
            string type;
            return node.TryGetPropertyValueFromObject("type", out type) && type.ToLowerInvariant().Equals("object");
        }

        public IList<Func<ParseTreeNode, bool>> GetValidatorRule(ParseTreeNode node)
        {
            var bootStrapper = BootstrapperLocator.Bootstrapper;

            //Second go through each property
            ParseTreeNode properties;
            var propertiesNode = node.TryGetPropertyValueFromObject("properties", out properties);

            var rules = new List<Func<ParseTreeNode, bool>>();
            foreach (var childNode in properties.ChildNodes)
            {
                var validatorRuleFactory = bootStrapper.GetValidatorRuleFactoryLocator().Locate(childNode);
                var generatedRules = validatorRuleFactory.GetValidatorRule(childNode);
                rules.AddRange(generatedRules);
            }

            return rules;
        }
    }
}