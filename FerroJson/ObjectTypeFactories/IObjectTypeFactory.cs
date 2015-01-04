using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson.ObjectTypeFactories
{
    public interface IObjectTypeFactory
    {
        bool BuildsObject(ParseTreeNode node);
        IList<Func<ParseTreeNode, bool>> BuildRules(ParseTreeNode node);
    }
}
