using System;
using System.Collections.Generic;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public class Maximum : IValidatorRuleFactory
    {
        private const string PropertyName = "maximum";
        private const string ExclusiveMaxPropertyName = "exclusiveMaximum";

        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get {return new [] {JsonSchema.SchemaVersion.V4};} }

        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            //First get the definition 
            var propertyDefinitioNode = jsonSchemaProperty.ChildNodes[1]; 
            
            //Are we dealing with an integer or a number, if not then we can't create a rule for this.
            string type;
            if (propertyDefinitioNode.TryGetPropertyValueFromObject("type", out type))
            {
                if (String.IsNullOrEmpty(type) || !(type.ToLowerInvariant().Equals("integer") || type.ToLowerInvariant().Equals("number")))
                {
                    return false;
                }
            }
            
            return propertyDefinitioNode.HasProperty(PropertyName);
        }

        public IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            //First get the definition 
            var propertyDefinitioNode = jsonSchemaProperty.ChildNodes[1]; 
            var propertyName = jsonSchemaProperty.GetPropertyName();

            //Then get the maximum value allowed according to the schema
            var maximumValue = propertyDefinitioNode.GetPropertyValueFromObject<float>(PropertyName);
            bool exclusiveMaximum;
            propertyDefinitioNode.TryGetPropertyValueFromObject(ExclusiveMaxPropertyName, out exclusiveMaximum);

            //Return validation rule
            Func<ParseTreeNode, IPropertyValidationResult> rule = property =>
            {
                float value;
                if (!property.TryGetValue(out value))
                {
                    return new PropertyValidationResult()
                    {
                        Error = "Cannot validate maximum. Input value is not numeric."
                    };
                }
                if (exclusiveMaximum ? value < maximumValue : value <= maximumValue)
                {
                    return new PropertyValidationResult()
                    {
                        Error = String.Format("Input value '{0}' is greater than maximumValue {1}.", value, maximumValue)
                    };
                }
                return null;
            };

            return new Dictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> { { propertyName, new[] { rule } } };
        }
    }
}
