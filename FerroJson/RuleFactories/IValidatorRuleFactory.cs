using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactory
    {
        IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; }
        bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
        IList<Func<ParseTreeNode, bool>> GetValidatorRule(ParseTreeNode jsonSchemaProperty);
    }
}