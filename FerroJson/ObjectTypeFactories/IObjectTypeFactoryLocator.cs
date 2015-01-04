using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace FerroJson.ObjectTypeFactories
{
    public interface IObjectTypeFactoryLocator
    {
        IObjectTypeFactory Locate(ParseTreeNode node);
    }

    public class DefaultObjectTypeFactoryLocator : IObjectTypeFactoryLocator
    {
        private readonly IEnumerable<IObjectTypeFactory> _objectTypeFactories;

        public DefaultObjectTypeFactoryLocator(IEnumerable<IObjectTypeFactory> objectTypeFactories)
        {
            _objectTypeFactories = objectTypeFactories;
        }

        public IObjectTypeFactory Locate(ParseTreeNode node)
        {
            return _objectTypeFactories.FirstOrDefault(f => f.BuildsObject(node));
        }
    }
}
