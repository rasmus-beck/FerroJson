using System;
using System.Collections.Generic;
using FerroJson.Bootstrapper;
using Irony.Parsing;

namespace FerroJson
{
    public class JsonSchemaFactory
    {
        public static IJsonSchema GetSchema(ParseTree jsonSchemaAst)
        {
            //ToDo... check cache first

            var bootStrapper = BootstrapperLocator.Bootstrapper;
            var validator = bootStrapper.GetValidatorLocator().GetValidator(jsonSchemaAst);

            var propertyRules = new SortedDictionary<string, IList<Func<object, bool>>>();
            //TODO: Next run through Json schema abstract syntax tree to build the property rules dictionary. 
            //Find a suitable key based on location in document, this should also support arrays

            return new JsonSchema(propertyRules);

        }
    }
}
