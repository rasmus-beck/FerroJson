using System;
using System.Collections.Generic;

namespace FerroJson
{
	public interface IProperty
	{
		IEnumerable<Func<dynamic, string>> Rules { get; set; }
		string Description { get; set; }
		string Name { get; set; }
	}
}