using System.Collections.Generic;

namespace FerroJson
{
    public class JsonSchema : IJsonSchema
    {
        private readonly IDictionary<string, IProperty> _propertyRules;

        public JsonSchema(IDictionary<string, IProperty> propertyRules)
        {
            _propertyRules = propertyRules;
        }

	    public IDictionary<string, IProperty> PropertyRules
	    {
		    get { return _propertyRules; }
	    }
    }
}
