using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Bootstrapper;
using FerroJson.PropertyRuleFactories;
using Irony.Parsing;

namespace FerroJson.ObjectTypeFactories
{
    public class Object : IObjectTypeFactory
    {
        private readonly IEnumerable<IPropertyValidatorRuleFactory> _ruleFactories;

        public Object(IEnumerable<IPropertyValidatorRuleFactory> ruleFactories)
        {
            _ruleFactories = ruleFactories;
        }

        public bool BuildsObject(ParseTreeNode node)
        {
            var typeNode = node.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "type"));
            if (null != typeNode && typeNode.ChildNodes.Count == 2)
            {
                var type = typeNode.ChildNodes[1].Token.ValueString;
                return type.Equals("object");
            }

            return false;
        }

        public IList<Func<ParseTreeNode, bool>> BuildRules(ParseTreeNode node)
        {
            var bootStrapper = BootstrapperLocator.Bootstrapper;

            //First build rules for the specific node
            var ruleFactories = _ruleFactories.Where(f => f.CanCreateValidatorRule(node));
            var rules = ruleFactories.Select(propertyValidatorRuleFactory => propertyValidatorRuleFactory.GetValidatorRule(node)).ToList();

            //Second go through each property
            var propertiesNode = node.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "properties"));

            if (null != propertiesNode && propertiesNode.ChildNodes.Count == 2)
            {
                var properties = propertiesNode.ChildNodes[1];
                foreach (var childNode in properties.ChildNodes)
                {
                    var objectTypeFactory = bootStrapper.GetObjectTypeFactoryLocator().Locate(childNode);
                    var generatedRules = objectTypeFactory.BuildRules(node);
                    rules.AddRange(generatedRules);
                }  
            }

            return rules;
        }
    }
}