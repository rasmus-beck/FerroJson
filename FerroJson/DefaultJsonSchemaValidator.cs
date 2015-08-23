using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
				var rules = schema.PropertyRules[nameSpace + property];
				if (null == rules)
					continue;

				var propertyErrors = rules.Rules.Select(rule => rule(value) as String).Where(result => !String.IsNullOrEmpty(result)).ToList();
				var validationError = new PropertyValidationError
				{
					Errors = propertyErrors,
					PropertyDescription = rules.Description,
					AttemptedValue = value
				};

				errors.AddRange(new[] { validationError });
			}

			return errors;
		}

		/*
		private IEnumerable<IPropertyValidationError> ValidateProperty(dynamic node, string nameSpace)
		{
			var propertyName = node.GetPropertyName();
			var propertyValue = node.GetPropertyValueNode();
			Console.WriteLine(nameSpace + propertyName);

			var rules = _schemaRules[nameSpace + propertyName];

			var errors = rules.Rules.Select(rule => rule(node)).Where(result => !String.IsNullOrEmpty(result)).ToList();
			var validationError = new PropertyValidationError()
			{
				Errors = errors,
				PropertyDescription = rules.Description,
				AttemptedValue = propertyValue.Token.Value
			};

			return new[] { validationError };
		}

		private IEnumerable<IPropertyValidationError> ValidateArray(ParseTreeNode node, string nameSpace)
		{
			throw new NotImplementedException();
		}*/
	}
}