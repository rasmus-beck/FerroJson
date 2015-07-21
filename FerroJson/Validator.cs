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
			// Move parsing to IJsonParser
            var jsonGrammar = new JsonGrammar();
            var jsonParser = new Parser(jsonGrammar);

            var jsonDocAst = jsonParser.Parse(jsonDocument);

            if (jsonDocAst.HasErrors())
            {
                var messages = jsonDocAst.ParserMessages.Select(parserMessage => parserMessage.Message).Aggregate((current, next) => current + Environment.NewLine + next);
                throw new ArgumentException(messages);
            }
			// Move parsing to IJsonParser - end

			var bootStrapper = BootstrapperLocator.Bootstrapper;
            var jsonSchemaFactory = bootStrapper.GetJsonSchemaFactory();

            var schemaHash = jsonSchema.GetHashCode().ToString(CultureInfo.InvariantCulture); //Move hashing to cache
			var schema = jsonSchemaFactory.GetSchema(jsonSchema, schemaHash);

			errors = new List<IPropertyValidationError>(); //Refactor errors to be dynamic dictionary to be able to mirror innput format
            return null == schema || schema.TryValidate(jsonDocAst, out errors); //Rewrite schema validator to just take string input
        }
    }
}