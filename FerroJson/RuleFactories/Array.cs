using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Array : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return false;
        }

        public IList<Func<ParseTreeNode, bool>> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return new Func<ParseTreeNode, bool>[0];
        }
    }
}