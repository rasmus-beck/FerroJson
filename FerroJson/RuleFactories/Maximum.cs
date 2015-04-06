using System;
using System.Collections.Generic;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
	public class Maximum : IValidatorRuleFactory
    {
        public IList<JsonSchema.SchemaVersion> SupportedSchemaVersions
        {
            get {return new [] {JsonSchema.SchemaVersion.V4};}
            protected set { throw new NotImplementedException(); }
        }

		public bool CanCreateValidatorRule(dynamic propertyDefinition)
        {
			return propertyDefinition.maximum.HasValue;
        }

		public Func<ParseTreeNode, string> GetValidatorRules(dynamic propertyDefinition)
		{
            //Get the maximum value allowed according to the schema
            var maximumValue = propertyDefinition.maximum;
			var exclusiveMaximum = propertyDefinition.exclusiveMaximum.HasValue ? propertyDefinition.exclusiveMaximum : false;

            //Return validation rule
            Func<ParseTreeNode, string> rule = property =>
            {
                float value;
                if (!property.TryGetValue(out value))
                {
	                return "Cannot validate maximum. Input value is not numeric.";
                }
                if (exclusiveMaximum ? value >= maximumValue : value > maximumValue)
                {
                    return String.Format("Input value '{0}' is greater than maximumValue {1}.", value, maximumValue);
                }
                return null;
            };

            return rule;
        }
    }
}
