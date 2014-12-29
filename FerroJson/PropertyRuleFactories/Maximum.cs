using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.PropertyRuleFactories
{
    public class Maximum : IPropertyValidatorRuleFactory
    {
        private const string PropertyName = "maximum";

        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions { get {return new [] {JsonSchema.SchemaVersion.V4};} }

        public bool CanCreateValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            return jsonSchemaProperty.ChildNodes.SelectMany(x => x.ChildNodes).Any(y => y.Token.ValueString == PropertyName);
        }

        public Func<object, bool> GetValidatorRule(ParseTreeNode jsonSchemaProperty)
        {
            var maximumProperty = jsonSchemaProperty.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == PropertyName));

            if (null == maximumProperty)
            {
                return x => true;
            }

            var maximumValue = maximumProperty.ChildNodes[1].Token.ValueString;

            var maxVal = float.Parse(maximumValue);

            return value =>
            {
                int intValue;
                if (!Int32.TryParse(value.ToString(), out intValue))
                {
                    return false;
                }

                return intValue <= maxVal;
            };
        }
    }
}
