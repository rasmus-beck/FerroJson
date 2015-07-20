using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FerroJson.Bootstrapper;
using Irony.Parsing;

namespace FerroJson
{
    public class Validator
    {
		public bool Validate(string jsonDocument, string jsonSchema, out IEnumerable<IPropertyValidationError> errors)
        {
            var jsonGrammar = new JsonGrammar();
            var jsonParser = new Parser(jsonGrammar);

            var jsonDocAst = jsonParser.Parse(jsonDocument);

            if (jsonDocAst.HasErrors())
            {
                var messages = jsonDocAst.ParserMessages.Select(parserMessage => parserMessage.Message).Aggregate((current, next) => current + Environment.NewLine + next);
                throw new ArgumentException(messages);
            }

			var bootStrapper = BootstrapperLocator.Bootstrapper;
            var jsonSchemaFactory = bootStrapper.GetJsonSchemaFactory();

            var schemaHash = jsonSchema.GetHashCode().ToString(CultureInfo.InvariantCulture);
			var schema = jsonSchemaFactory.GetSchema(jsonSchema, schemaHash);

			errors = new List<IPropertyValidationError>();
            return null == schema || schema.TryValidate(jsonDocAst, out errors);
        }
    }
}