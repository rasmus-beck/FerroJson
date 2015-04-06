using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactory
    {
        IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; }
		bool CanCreateValidatorRule(dynamic propertyDefinition);
		Func<ParseTreeNode, string> GetValidatorRules(dynamic propertyDefinition);
    }
}