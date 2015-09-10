using System;

namespace FerroJson.Extensions
{
	public static class StringExtensions
	{
		public static string AppendToNameSpace(this string nameSpace, string nameSpaceToAppend)
		{
			if (String.IsNullOrEmpty(nameSpace))
			{
				return nameSpaceToAppend;
			}

			return nameSpace + "." + nameSpaceToAppend;
		}
	}
}
