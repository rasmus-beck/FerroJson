using System.Collections.Generic;
using Irony.Parsing;
using System.Linq;

namespace FerroJson.JsonSchemaV4
{
    public class V4Validator : IValidator
    {
        private const string SchemaIdentifier = "http://json-schema.org/draft-04/schema#";
        private readonly IEnumerable<IJsonSchemaV4ValidatorRule> _rules;

        public V4Validator(IEnumerable<IJsonSchemaV4ValidatorRule> rules)
        {
            _rules = rules;
        }

        public bool CanValidate(ParseTree jsonSchema)
        {
            var schemaIdentifier = jsonSchema.Root.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "$schema"));

            if (null == schemaIdentifier) return false;
            
            var schemaIdentifierProperty = schemaIdentifier.ChildNodes.FirstOrDefault(y => y.Token.ValueString != "$schema");
            return null != schemaIdentifierProperty && schemaIdentifierProperty.Token.ValueString == SchemaIdentifier;
        }

        public bool Validate(ParseTree jsonDoc, ParseTree jsonSchema)
        {
            return true;
        }
    }
}
