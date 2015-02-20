using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
    public class JsonSchema : IJsonSchema
    {
        public enum SchemaVersion
        {
            V1,
            V2,
            V3,
            V4,
        };

        private readonly IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> _schemaRules;
        private readonly bool _allowAdditionalProperties;
        private readonly IList<string> _requiredProperties;

        public JsonSchema(IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> schemaRules, bool allowAdditionalProperties, IList<string> requiredProperties)
        {
            _schemaRules = schemaRules;
            _allowAdditionalProperties = allowAdditionalProperties;
            _requiredProperties = requiredProperties;
        }

        public bool TryValidate(ParseTree jsonDoc, out IEnumerable<string> validationErrors)
        {
            //TODO: Run through json document forward only, and apply rules that match each
            // remember checks for the special cases _allowAdditionalProperties and _requiredProperties

            throw new NotImplementedException();
        }
    }
}
