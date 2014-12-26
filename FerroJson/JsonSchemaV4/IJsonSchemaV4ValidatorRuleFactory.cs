using System;
using Irony.Parsing;

namespace FerroJson.JsonSchemaV4
{
    public interface IJsonSchemaV4ValidatorRuleFactory
    {
        bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty);
        Func<object, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty);
    }
}