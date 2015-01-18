using System;
using System.Collections.Generic;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.PropertyRuleFactories
{
    public class Maximum : IPropertyValidatorRuleFactory
    {
        private const string PropertyName = "maximum";
        private const string ExclusiveMaxPropertyName = "exclusiveMaximum";

        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get {return new [] {JsonSchema.SchemaVersion.V4};} }

        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return jsonSchemaProperty.HasProperty(PropertyName);
        }

        public Func<ParseTreeNode, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            //Are we dealing with an integer or a number, if not then raise an error.
            var type = jsonSchemaProperty.GetPropertyValueFromObject<string>("type");

            if (!String.IsNullOrEmpty(type) && !(type.ToLowerInvariant().Equals("integer") || type.ToLowerInvariant().Equals("number")))
            {
                throw new Exception("A maximum can only be defined on properties of type 'number' or 'integer'.");
            }
            
            //Then get the maximum value allowed according to the schema
            var maximumValue = jsonSchemaProperty.GetPropertyValueFromObject<float>(PropertyName);
            bool exclusiveMaximum;
            jsonSchemaProperty.TryGetPropertyValueFromObject(ExclusiveMaxPropertyName, out exclusiveMaximum);

            //Return validation rule
            return property =>
            {
                float value;
                if (!property.TryGetValue(out value))
                {
                    return false;
                }
                return exclusiveMaximum ? value < maximumValue : value <= maximumValue;
            };
        }
    }
}
