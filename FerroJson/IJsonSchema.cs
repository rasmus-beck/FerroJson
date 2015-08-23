using System.Collections.Generic;

namespace FerroJson
{
    public interface IJsonSchema
    {
		IDictionary<string, IProperty> PropertyRules { get; }
    }
}
