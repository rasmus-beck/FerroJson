using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Properties : ValidatorRuleFactory
    {
        public override IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; protected set; }
        public override bool CanCreateValidatorRule(ParseTreeNode node)
        {
            return IsNamed(node, "properties");
        }

        public override IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(ParseTreeNode node)
        {
            var propertyDefinitionNode = GetPropertyDefinitionNode(node);
            return GetRulesFromObject(propertyDefinitionNode.ChildNodes, String.Empty);
        }
    }
}
