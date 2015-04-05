using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactoryLocator
    {
        IEnumerable<IValidatorRuleFactory> Locate(ParseTreeNode node);
    }

    public class DefaultValidatorRuleFactoryLocator : IValidatorRuleFactoryLocator
    {
        private readonly IEnumerable<IValidatorRuleFactory> _validatorRuleFactories;

        public DefaultValidatorRuleFactoryLocator(IEnumerable<IValidatorRuleFactory> validatorRuleFactories)
        {
            _validatorRuleFactories = validatorRuleFactories;
        }

        public IEnumerable<IValidatorRuleFactory> Locate(ParseTreeNode node)
        {
            return _validatorRuleFactories.Where(f => f.CanCreateValidatorRule(node));
        }
    }



	public interface IReferenceTypeRuleFactoryLocator
	{
		IReferenceTypeRuleFactory Locate(ParseTreeNode node);
	}

	public class DefaultReferenceTypeRuleFactoryLocator : IReferenceTypeRuleFactoryLocator
	{
		private readonly IEnumerable<IReferenceTypeRuleFactory> _referenceTypeRuleFactories;

		public DefaultReferenceTypeRuleFactoryLocator(IEnumerable<IReferenceTypeRuleFactory> referenceTypeRuleFactories)
		{
			_referenceTypeRuleFactories = referenceTypeRuleFactories;
		}

		public IReferenceTypeRuleFactory Locate(ParseTreeNode node)
		{
			return _referenceTypeRuleFactories.FirstOrDefault(f => f.CanCreateValidatorRule(node));
		}
	}
}
