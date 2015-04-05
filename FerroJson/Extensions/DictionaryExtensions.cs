using System.Collections.Generic;
using System.Linq;

namespace FerroJson.Extensions
{
	public static class DictionaryExtensions
	{
		public static IDictionary<string, IProperty> AddRange(this IDictionary<string, IProperty> source,
			IDictionary<string, IProperty> other)
		{
			if (null == source && null != other)
				return other;

			if (null != source && null == other)
				return source;

			if (null == source && null == other)
				return null;

			var newDictionary = new Dictionary<string, IProperty>(source);

			foreach (var property in other)
			{
				if (newDictionary.ContainsKey(property.Key))
				{
					newDictionary[property.Key].Rules = newDictionary[property.Key].Rules.Concat(property.Value.Rules).ToList();
				}
				else
				{
					newDictionary.Add(property.Key, property.Value);
				}
			}

			return newDictionary;
		}
	}
}
