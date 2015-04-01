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

        private readonly IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> _schemaRules;
        private readonly bool _allowAdditionalProperties;
        private readonly IList<string> _requiredProperties;

        public JsonSchema(IDictionary<string, IList<Func<ParseTreeNode, IPropertyValidationResult>>> schemaRules, bool allowAdditionalProperties, IList<string> requiredProperties)
        {
            _schemaRules = schemaRules;
            _allowAdditionalProperties = allowAdditionalProperties;
            _requiredProperties = requiredProperties;
        }

		public bool TryValidate(ParseTree jsonDoc, out IEnumerable<IPropertyValidationResult> validationErrors)
        {
            //TODO: Run through json document forward only, and apply rules that match each
            // Checks should run in parallel.
            // remember checks for the special cases _allowAdditionalProperties and _requiredProperties

			validationErrors = ValidateObject(jsonDoc.Root, "");

	        return validationErrors == null || !validationErrors.Any();
        }

		public IEnumerable<IPropertyValidationResult> ValidateObject(ParseTreeNode node, string nameSpace)
	    {
			var errors = new List<IPropertyValidationResult>();
		    foreach (var n in node.ChildNodes.Where( n => n.Term.Name == "Property"))
		    {
			    var propertyErrors = ValidateProperty(n, nameSpace);
				errors.AddRange(propertyErrors);
		    }

			return errors;
	    }

		public IEnumerable<IPropertyValidationResult> ValidateProperty(ParseTreeNode node, string nameSpace)
	    {
			var propertyName = node.GetPropertyName();
			var propertyValue = node.GetPropertyValueNodeFromObject(propertyName);
			Console.WriteLine(nameSpace + propertyName);

		    var rules = _schemaRules[nameSpace + propertyName];

			return rules.Select(rule => rule(node)).Where(result => result != null).ToList();
	    }
    }
}
