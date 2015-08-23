using System;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactory
    {
		bool CanCreateValidatorRule(dynamic propertyDefinition);
		Func<dynamic, string> GetValidatorRules(dynamic propertyDefinition);
    }
}