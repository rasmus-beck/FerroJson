using System;
using System.Collections.Generic;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class ObjectType : ValidatorRuleFactory
    {
        public override IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; protected set; }
        public override bool CanCreateValidatorRule(ParseTreeNode node)
        {
            return IsObjectNode(node);
        }

        public override IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(ParseTreeNode node)
        {
            //First get name of the property - will be null if we are dealing with the root node
            string propertyName;
            var objectNode = node;
            if (node.TryGetPropertyName(out propertyName))
            {
                objectNode = GetPropertyDefinitionNode(node);
            }

            return GetRulesFromObject(objectNode.ChildNodes, propertyName);
        }
    }
}