using System;
using System.Collections.Generic;
using FerroJson.PropertyRuleFactories;
using Irony.Parsing;

namespace FerroJson.ObjectTypeFactories
{
    public class Array : IObjectTypeFactory
    {
        private readonly IEnumerable<IPropertyValidatorRuleFactory> _ruleFactories;

        public Array(IEnumerable<IPropertyValidatorRuleFactory> ruleFactories)
        {
            _ruleFactories = ruleFactories;
        }

        public bool BuildsObject(ParseTreeNode node)
        {
            return false;
        }

        public IList<Func<ParseTreeNode, bool>> BuildRules(ParseTreeNode node)
        {
            return new Func<ParseTreeNode, bool>[0];
        }
    }
}