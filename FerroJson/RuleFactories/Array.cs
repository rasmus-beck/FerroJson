using System;
using System.Collections.Generic;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Array : ValidatorRuleFactory
    {
        public override IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; protected set; }
        public override bool CanCreateValidatorRule(ParseTreeNode node)
        {
            return IsArrayNode(node);
        }

        public override IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(ParseTreeNode node)
        {
            var propertyDefinition = GetPropertyDefinitionNode(node);
            var items = propertyDefinition.GetPropertyValueNodeFromObject("items");

            return GetRulesFromObject(items, node.GetPropertyName());
        }
    }
}