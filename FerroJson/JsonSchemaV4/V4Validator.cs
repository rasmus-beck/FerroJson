﻿using System.Collections.Generic;
using Irony.Parsing;
using System.Linq;

namespace FerroJson.JsonSchemaV4
{
    public class V4Validator : IValidator
    {
        private const string SchemaIdentifier = "http://json-schema.org/draft-04/schema#";
        public IEnumerable<IJsonSchemaV4ValidatorRuleFactory> RuleFactories { get; private set; }

        public V4Validator(IEnumerable<IJsonSchemaV4ValidatorRuleFactory> ruleFactories)
        {
            RuleFactories = ruleFactories;
        }

        public bool CanValidate(ParseTree jsonSchema)
        {
            //Does this need to be a better check? Right now we only look for the existence of a property value that matches the schema identifier.
            var schemaIdentifier = jsonSchema.Root.ChildNodes.SelectMany(x => x.ChildNodes).FirstOrDefault(y => y.Token.ValueString == SchemaIdentifier);
            return null != schemaIdentifier;
        }
    }
}
