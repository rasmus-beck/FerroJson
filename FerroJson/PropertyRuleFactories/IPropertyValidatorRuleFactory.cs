using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.PropertyRuleFactories
{
    public interface IPropertyValidatorRuleFactory
    {
        IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; }
        bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
        Func<ParseTreeNode, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty);
    }
}