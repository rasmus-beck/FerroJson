using System.Collections.Generic;
using FerroJson.Bootstrapper;

namespace FerroJson
{
    public class Validator
    {
		public bool Validate(string jsonDocument, string jsonSchema, out IEnumerable<IPropertyValidationError> errors)
        {
			var bootStrapper = BootstrapperLocator.Bootstrapper;
            var jsonSchemaValidator = bootStrapper.GetJsonSchemaValidator();
			return jsonSchemaValidator.TryValidate(jsonDocument, jsonSchema, out errors);
        }
    }
}