using System;

namespace FerroJson.RuleFactories
{
	public class Maximum : IValidatorRuleFactory
    {
		public bool CanCreateValidatorRule(dynamic propertyDefinition)
        {
			return propertyDefinition.maximum.HasValue;
        }

		public Func<dynamic, string> GetValidatorRules(dynamic propertyDefinition)
		{
            //Get the maximum value allowed according to the schema
            var maximumValue = propertyDefinition.maximum;
			var exclusiveMaximum = propertyDefinition.exclusiveMaximum.HasValue ? propertyDefinition.exclusiveMaximum : false;

            //Return validation rule
            Func<dynamic, string> rule = property =>
            {				
                float value;
				try
				{
					value = (float)property;
				}
				catch (Exception)
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
