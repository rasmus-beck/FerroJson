namespace FerroJson
{
    public interface IJsonSchemaFactory
    {
		IJsonSchema GetSchema(dynamic jsonSchema, string schemaHash);
    }

    public class DefaultJsonSchemaFactory : IJsonSchemaFactory
    {
	    private readonly ObjectReferenceTypeRuleFactory _rootFactory;
	    private readonly IJsonSchemaCacheProvider _cache;

	    public DefaultJsonSchemaFactory(ObjectReferenceTypeRuleFactory rootFactory, IJsonSchemaCacheProvider cache)
		{
			_rootFactory = rootFactory;
			_cache = cache;
		}

		public IJsonSchema GetSchema(dynamic jsonSchema, string schemaHash)
        {
			var schema = _cache.Get(schemaHash);
            if (null != schema)
                return schema;

			var rules = _rootFactory.GetValidatorRules(jsonSchema);

            schema = new JsonSchema(rules);
            _cache.Set(schemaHash, schema);

            return schema;
        }
    }
}
