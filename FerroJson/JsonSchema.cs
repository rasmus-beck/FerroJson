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

        private readonly SortedDictionary<string, IList<Func<object, bool>>> _schemaProperties;
        private readonly bool _allowAdditionalProperties;
        private readonly IList<string> _requiredProperties;

        public JsonSchema(SortedDictionary<string, IList<Func<object, bool>>> schemaProperties, bool allowAdditionalProperties, IList<string> requiredProperties)
        {
            _schemaProperties = schemaProperties;
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
