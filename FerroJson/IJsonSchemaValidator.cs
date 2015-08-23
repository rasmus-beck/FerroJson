using System.Collections.Generic;

namespace FerroJson
{
	public interface IJsonSchemaValidator
	{
		bool TryValidate(string jsonDocument, string jsonSchema, out IEnumerable<IPropertyValidationError> validationErrors);
	}
}