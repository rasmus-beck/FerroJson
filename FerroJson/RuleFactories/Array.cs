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

        public IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            throw new NotImplementedException();
        }
    }
}