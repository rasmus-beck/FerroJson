using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
	public class Property : IProperty
	{
		public IList<Func<ParseTreeNode, string>> Rules { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
	}
}