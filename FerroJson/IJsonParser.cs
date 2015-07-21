using System;
using System.Linq;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson
{
	public interface IJsonParser
	{
		DynamicDictionary.DynamicDictionary Parse(string jsonDocument);
	}

	public class DefaultJsonParser : IJsonParser
	{
		private readonly Parser _jsonParser;

		public DefaultJsonParser()
		{
			var jsonGrammar = new JsonGrammar();
			_jsonParser = new Parser(jsonGrammar);
		}
		public DynamicDictionary.DynamicDictionary Parse(string jsonDocument)
		{
			var jsonDocAst = _jsonParser.Parse(jsonDocument);

			if (jsonDocAst.HasErrors())
			{
				var messages = jsonDocAst.ParserMessages.Select(parserMessage => parserMessage.Message).Aggregate((current, next) => current + Environment.NewLine + next);
				throw new ArgumentException(messages);
			}

			return jsonDocAst.AsDynamicDictionary();
		}
	}
}
