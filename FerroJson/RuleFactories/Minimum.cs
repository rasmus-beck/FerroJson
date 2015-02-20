using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Minimum : ValidatorRuleFactory
    {
        public override IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get; protected set; }
        public override bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return false;
        }

        public override IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRules(ParseTreeNode jsonSchemaProperty)
        {
            throw new NotImplementedException();
        }
    }
}
