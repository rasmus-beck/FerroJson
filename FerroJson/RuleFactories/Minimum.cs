using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Minimum : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; private set; }
        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return false;
        }

        public IList<Func<ParseTreeNode, bool>> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            throw new NotImplementedException();
        }
    }
}
