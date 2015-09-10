using System;
using System.Collections.Generic;
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

		public bool TryValidate(string jsonDocument, string jsonSchema, out IEnumerable<IPropertyValidationError> validationErrors)
		{
			dynamic documentDictionary = _jsonParser.Parse(jsonDocument);
			dynamic schemaDictionary = _jsonParser.Parse(jsonSchema);

			var schemaHash = jsonSchema.GetHashCode().ToString(CultureInfo.InvariantCulture);

			var schema = _jsonSchemaFactory.GetSchema(schemaDictionary, schemaHash);

			validationErrors = ValidateObject(documentDictionary, String.Empty, schema);
			return validationErrors == null || !validationErrors.Any();
		}

		private IEnumerable<IPropertyValidationError> ValidateObject(dynamic node, string nameSpace, IJsonSchema schema)
		{
			var errors = new List<IPropertyValidationError>();

			foreach (string property in node)
			{
				dynamic value = node[property];

				if (value.Value is Array)
				{
					errors.AddRange(ValidateArray(value.Value, property, nameSpace, schema));
				}
				else if (value.Value is DynamicDictionary.DynamicDictionary)
				{
					errors.AddRange(ValidateObject(value.Value, nameSpace.AppendToNameSpace(property), schema));
				}
				else
				{
					errors.AddRange(ValidateProperty(value.Value, nameSpace.AppendToNameSpace(property), schema));
				}

				var isArray = value.Value is Array;
				var isObject = value.Value is DynamicDictionary.DynamicDictionary;
			}

			return errors;
		}

		private IEnumerable<IPropertyValidationError> ValidateProperty(dynamic value, string nameSpace, IJsonSchema schema)
		{
			if (!schema.PropertyRules.ContainsKey(nameSpace))
			{
				return new List<IPropertyValidationError>();
			}

			var rules = schema.PropertyRules[nameSpace];

			var propertyErrors = rules.Rules.Select(rule => rule(value) as String).Where(result => !String.IsNullOrEmpty(result)).ToList();

			if (propertyErrors.Any())
			{
				var validationError = new PropertyValidationError
				{
					Errors = propertyErrors,
					PropertyDescription = rules.Description,
					AttemptedValue = value
				};

				return new List<IPropertyValidationError> { validationError };
			}

			return new List<IPropertyValidationError>();
		}

		private IEnumerable<IPropertyValidationError> ValidateArray(dynamic array, string property, string nameSpace, IJsonSchema schema)
		{
			var errors = new List<IPropertyValidationError>();
			
			foreach (dynamic arrayItem in array)
			{
				if (arrayItem.GetType().IsValueType)
				{
					errors.AddRange(ValidateProperty(arrayItem, nameSpace.AppendToNameSpace(property), schema));
				}
				else
				{
					errors.AddRange(ValidateObject(arrayItem, nameSpace.AppendToNameSpace(property), schema));
				}
			}
			return errors;
		}
	}
}