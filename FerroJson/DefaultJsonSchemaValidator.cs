using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using FerroJson.Extensions;

namespace FerroJson
{
	public class DefaultJsonSchemaValidator : IJsonSchemaValidator
	{
		private readonly IJsonSchemaFactory _jsonSchemaFactory;
		private readonly IJsonParser _jsonParser;

		public DefaultJsonSchemaValidator(IJsonSchemaFactory jsonSchemaFactory, IJsonParser jsonParser)
		{
			_jsonSchemaFactory = jsonSchemaFactory;
			_jsonParser = jsonParser;
		}

		public bool TryValidate(string jsonDocument, string jsonSchema, out dynamic validationErrors)
		{
			dynamic documentDictionary = _jsonParser.Parse(jsonDocument);
			dynamic schemaDictionary = _jsonParser.Parse(jsonSchema);

			var schemaHash = jsonSchema.GetHashCode().ToString(CultureInfo.InvariantCulture);

			var schema = _jsonSchemaFactory.GetSchema(schemaDictionary, schemaHash);

			dynamic result = ValidateObject(documentDictionary, String.Empty, schema);

			if (null != result)
			{
				validationErrors = result.ToDictionary();
				return false;
			}

			validationErrors = null;
			return true;
		}

		private dynamic ValidateObject(dynamic node, string nameSpace, IJsonSchema schema)
		{
			var validationError = new DynamicDictionary.DynamicDictionary();

			foreach (string property in node)
			{
				dynamic value = node[property];
				dynamic validationResult = null;

				if (value.Value is Array)
				{
					validationResult = ValidateArray(value.Value, property, nameSpace, schema);
				}
				else if (value.Value is DynamicDictionary.DynamicDictionary)
				{
					validationResult = ValidateObject(value.Value, nameSpace.AppendToNameSpace(property), schema);
				}
				else
				{
					validationResult = ValidateProperty(value.Value, nameSpace.AppendToNameSpace(property), schema);
				}

				if (null != validationResult)
				{
					validationError[property] = validationResult;
				}
			}

			if (validationError.Count > 0)
			{
				return validationError as dynamic;
			}

			return null;
		}

		private dynamic ValidateProperty(dynamic value, string nameSpace, IJsonSchema schema)
		{
			if (!schema.PropertyRules.ContainsKey(nameSpace))
			{
				return null;
			}

			var rules = schema.PropertyRules[nameSpace];
			var propertyErrors = rules.Rules.Select(rule => rule(value) as String).Where(result => !String.IsNullOrEmpty(result)).ToList();

			if (propertyErrors.Any())
			{
				var validationError = new DynamicDictionary.DynamicDictionary();
				validationError["Errors"] = propertyErrors;
				validationError["Description"] = rules.Description;
				validationError["AttemptedValue"] = value;
				return validationError;
			}

			return null;
		}

		private dynamic ValidateArray(dynamic array, string property, string nameSpace, IJsonSchema schema)
		{
			var items = new List<dynamic>(array.Length);
			
			for (var i = 0; i < array.Length; i++)
			{
				dynamic arrayItem = array[i];
				dynamic error = null;

				if (arrayItem.GetType().IsValueType)
				{
					error = ValidateProperty(arrayItem, nameSpace.AppendToNameSpace(property), schema);
				}
				else
				{
					error = ValidateObject(arrayItem, nameSpace.AppendToNameSpace(property), schema);
				}

				if (null != error)
				{
					error.ArrayIndex = i;
					items.Add(error);
				}
			}

			if (items.Any())
			{
				var validationError = new DynamicDictionary.DynamicDictionary();
				// TODO: missing description
				validationError["Items"] = items;
				return validationError;
			}

			return null;
		}
	}
}