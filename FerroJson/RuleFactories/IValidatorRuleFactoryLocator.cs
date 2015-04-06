using System.Collections.Generic;
using System.Linq;

namespace FerroJson.RuleFactories
{
    public interface IReferenceTypeRuleFactoryLocator
	{
		IReferenceTypeRuleFactory Locate(dynamic propertyDefinition);
	}

	public class DefaultReferenceTypeRuleFactoryLocator : IReferenceTypeRuleFactoryLocator
	{
		private readonly IEnumerable<IReferenceTypeRuleFactory> _referenceTypeRuleFactories;

		public DefaultReferenceTypeRuleFactoryLocator(IEnumerable<IReferenceTypeRuleFactory> referenceTypeRuleFactories)
		{
			_referenceTypeRuleFactories = referenceTypeRuleFactories;
		}

		public IReferenceTypeRuleFactory Locate(dynamic propertyDefinition)
		{
			return _referenceTypeRuleFactories.FirstOrDefault(f => f.CanCreateValidatorRule(propertyDefinition));
		}
	}
}
