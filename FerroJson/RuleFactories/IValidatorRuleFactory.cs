using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactory
    {
        IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; }
        bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
		Func<ParseTreeNode, string> GetValidatorRules(string propertyName, ParseTreeNode jsonSchemaProperty);
    }
}