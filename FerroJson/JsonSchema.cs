using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
    public class JsonSchema : IJsonSchema
    {
        public SortedDictionary<string, IList<Func<object, bool>>> SchemaProperties { get; private set; }

        public JsonSchema(SortedDictionary<string, IList<Func<object, bool>>> schemaProperties)
        {
            SchemaProperties = schemaProperties;
        }

        public bool TryValidate(ParseTree jsonDoc, out IEnumerable<string> validationErrors)
        {
            throw new NotImplementedException();
        }
    }
}
