using System;
using System.Collections.Generic;
using FerroJson.ObjectRuleFactories;
using FerroJson.PropertyRuleFactories;
using Irony.Parsing;

namespace FerroJson
{
    public interface IJsonSchemaFactory
    {
        IJsonSchema GetSchema(ParseTree jsonSchemaAst);
    }

    public class JsonSchemaFactory : IJsonSchemaFactory
    {
        private readonly IEnumerable<IPropertyValidatorRuleFactory> _propertyRuleFactories;
        private readonly IEnumerable<IObjectValidatorRuleFactory> _objectRuleFactories;

        public JsonSchemaFactory(IEnumerable<IPropertyValidatorRuleFactory> propertyRuleFactories, IEnumerable<IObjectValidatorRuleFactory> objectRuleFactories)
        {
            _propertyRuleFactories = propertyRuleFactories;
            _objectRuleFactories = objectRuleFactories;
        }

        public IJsonSchema GetSchema(ParseTree jsonSchemaAst)
        {
            //ToDo... check cache first

            var propertyRules = new SortedDictionary<string, IList<Func<object, bool>>>();
            //TODO: Next run through Json schema abstract syntax tree to build the property rules dictionary. 
            //Find a suitable key based on location in document, this should also support arrays

            var allowAdditionalProperties = true;
            var requiredProperties = new string[]{};

            return new JsonSchema(propertyRules, allowAdditionalProperties, requiredProperties);
        }
    }
}
