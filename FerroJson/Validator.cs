using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using Irony.Parsing;

namespace FerroJson
{
    public class Validator
    {
        public bool Validate(string jsonDocument, string jsonSchema, out IEnumerable<string> errors)
        {
            var jsonGrammar = new JsonGrammar();
            var jsonParser = new Parser(jsonGrammar);

            var jsonDocAst = jsonParser.Parse(jsonDocument);

            if (jsonDocAst.HasErrors())
            {
                var messages = jsonDocAst.ParserMessages.Select(parserMessage => parserMessage.Message).Aggregate((current, next) => current + Environment.NewLine + next);
                throw new ArgumentException(messages);
            }

            var jsonSchemaAst = jsonParser.Parse(jsonSchema);

            if (jsonSchemaAst.HasErrors())
            {
                var messages = jsonSchemaAst.ParserMessages.Select(parserMessage => parserMessage.Message).Aggregate((current, next) => current + Environment.NewLine + next);
                throw new ArgumentException(messages);
            }

            var bootStrapper = BootstrapperLocator.Bootstrapper;
            var jsonSchemaFactory = bootStrapper.GetJsonSchemaFactory();

            var schemaHash = jsonSchema.GetHashCode().ToString();

            var schema = jsonSchemaFactory.GetSchema(jsonSchemaAst, schemaHash);
            schema = jsonSchemaFactory.GetSchema(jsonSchemaAst, schemaHash);

            errors = new List<string>();
            return null == schema || schema.TryValidate(jsonDocAst, out errors);
        }
    }
}