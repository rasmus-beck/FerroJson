using System;
using System.Collections.Generic;
using System.Linq;
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
            return jsonSchemaProperty.ChildNodes.SelectMany(x => x.ChildNodes).Any(y => null != y.Token && y.Token.ValueString == PropertyName);
        }

        public Func<ParseTreeNode, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            //Are we dealing with an integer or a number, if not then raise an error.
            var typeProperty = jsonSchemaProperty.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "type"));
            if (null != typeProperty)
            {
                var typeValue = typeProperty.ChildNodes[1].Token.ValueString.ToLowerInvariant();
                if (!(typeValue.Equals("integer") || typeValue.Equals("number")))
                {
                    throw new Exception("A maximum can only be defined on properties of type 'number' or 'integer'.");
                }
            }
            
            //Then get the maximum value allowed according to the schema
            var maximumProperty = jsonSchemaProperty.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == PropertyName));

            if (null == maximumProperty)
            {
                return x => true;
            }

            var maximumValueString = maximumProperty.ChildNodes[1].Token.ValueString;
            var maximumValue = float.Parse(maximumValueString);

            //Last determine if this is an exclusive maximum. Default is false.
            var exclusiveMaximumProperty = jsonSchemaProperty.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == ExclusiveMaxPropertyName));
            var exclusiveMaximum = false;

            if (null != exclusiveMaximumProperty)
            {
                var exclusiveMaximumString = exclusiveMaximumProperty.ChildNodes[1].Token.ValueString;
                Boolean.TryParse(exclusiveMaximumString, out exclusiveMaximum);
            }

            //Return validation rule
            return property =>
            {
                var value = property.ChildNodes[1].Token.ValueString;

                float floatValue;
                if (!float.TryParse(value, out floatValue))
                {
                    return false;
                }

                return exclusiveMaximum ? floatValue < maximumValue : floatValue <= maximumValue;
            };
        }
    }
}
