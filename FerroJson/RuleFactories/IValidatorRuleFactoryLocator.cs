using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.RuleFactories
{
    public interface IValidatorRuleFactoryLocator
    {
        IValidatorRuleFactory Locate(ParseTreeNode node);
    }

    public class DefaultValidatorRuleFactoryLocator : IValidatorRuleFactoryLocator
    {
        private readonly IEnumerable<IValidatorRuleFactory> _validatorRuleFactories;

        public DefaultValidatorRuleFactoryLocator(IEnumerable<IValidatorRuleFactory> validatorRuleFactories)
        {
            _validatorRuleFactories = validatorRuleFactories;
        }

        public IValidatorRuleFactory Locate(ParseTreeNode node)
        {
            return _validatorRuleFactories.FirstOrDefault(f => f.CanCreateValidatorRule(node));
        }
    }
}
