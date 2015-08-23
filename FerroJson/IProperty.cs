using System;
using System.Collections.Generic;

namespace FerroJson
{
	public interface IProperty
	{
		IList<Func<dynamic, string>> Rules { get; set; }
		string Description { get; set; }
		string Name { get; set; }
	}
}