using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
    public interface IJsonSchema
    {
		bool TryValidate(ParseTree jsonDoc, out IEnumerable<IPropertyValidationError> validationErrors);
    }
}
