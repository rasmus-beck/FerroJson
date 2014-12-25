using System;
using System.Linq;
using FerroJson.Bootstrapper;
using Irony.Parsing;

namespace FerroJson
{
    public class Validator
    {
        public bool Validate(string jsonDocument, string jsonSchema)
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

            var validator = bootStrapper.GetValidatorLocator().GetValidator(jsonSchemaAst);

            return validator.Validate(jsonDocAst, jsonSchemaAst);
        }
    }
}