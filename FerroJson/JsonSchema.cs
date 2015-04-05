using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson
{
    public class JsonSchema : IJsonSchema
    {
        public enum SchemaVersion
        {
            V1,
            V2,
            V3,
            V4,
        };

        private readonly IDictionary<string, IProperty> _schemaRules;
        private readonly bool _allowAdditionalProperties;
        private readonly IList<string> _requiredProperties;

        public JsonSchema(IDictionary<string, IProperty> schemaRules, bool allowAdditionalProperties, IList<string> requiredProperties)
        {
            _schemaRules = schemaRules;
            _allowAdditionalProperties = allowAdditionalProperties;
            _requiredProperties = requiredProperties;
        }

		public bool TryValidate(ParseTree jsonDoc, out IEnumerable<IPropertyValidationError> validationErrors)
        {
            //TODO: Run through json document forward only, and apply rules that match each
            // Checks should run in parallel.
            // remember checks for the special cases _allowAdditionalProperties and _requiredProperties

			validationErrors = ValidateObject(jsonDoc.Root, "");

	        return validationErrors == null || !validationErrors.Any();
        }

		public IEnumerable<IPropertyValidationError> ValidateObject(ParseTreeNode node, string nameSpace)
	    {
			var errors = new List<IPropertyValidationError>();
		    foreach (var n in node.ChildNodes.Where( n => n.Term.Name == "Property"))
		    {
			    var propertyErrors = ValidateProperty(n, nameSpace);
				errors.AddRange(propertyErrors);
		    }

			return errors;
	    }

		public IEnumerable<IPropertyValidationError> ValidateProperty(ParseTreeNode node, string nameSpace)
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

	    public IEnumerable<IPropertyValidationError> ValidateArray(ParseTreeNode node, string nameSpace)
	    {
		    throw new NotImplementedException();
	    }
    }
}
