using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
	public interface IProperty
	{
		IList<Func<ParseTreeNode, string>> Rules { get; set; }
		string Description { get; set; }
		string Name { get; set; }
	}
}