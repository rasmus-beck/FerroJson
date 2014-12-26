using System.Collections.Generic;
using FerroJson.JsonSchemaV4;
using Irony.Parsing;

namespace FerroJson
{
    public interface IValidator
    {
        IEnumerable<IJsonSchemaV4ValidatorRuleFactory> RuleFactories { get;  }
        bool CanValidate(ParseTree jsonSchema);
    }
}
