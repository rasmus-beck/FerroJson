using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.PropertyRuleFactories
{
    public class Minimum : IPropertyValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            throw new NotImplementedException();
        }

        public Func<ParseTreeNode, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            throw new NotImplementedException();
        }
    }
}
