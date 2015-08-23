using System;
using System.Collections.Generic;

namespace FerroJson
{
	public class Property : IProperty
	{
		public IList<Func<dynamic, string>> Rules { get; set; }
		public string Description { get; set; }
		public string Name { get; set; }
	}
}