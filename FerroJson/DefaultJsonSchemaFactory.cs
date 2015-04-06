using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.Extensions;
using Irony.Parsing;

namespace FerroJson
{
    public interface IJsonSchemaFactory
    {
        IJsonSchema GetSchema(ParseTree jsonSchemaAst, string schemaHash);
    }

    public class DefaultJsonSchemaFactory : IJsonSchemaFactory
    {
        private readonly Dictionary<string, JsonSchema.SchemaVersion> _schemaVersionMap = new Dictionary <string, JsonSchema.SchemaVersion>
        {
            {"http://json-schema.org/draft-01/schema#", JsonSchema.SchemaVersion.V1},
            {"http://json-schema.org/draft-02/schema#", JsonSchema.SchemaVersion.V2},
            {"http://json-schema.org/draft-03/schema#", JsonSchema.SchemaVersion.V3},
            {"http://json-schema.org/draft-04/schema#", JsonSchema.SchemaVersion.V4}
        };

	    private readonly ObjectReferenceTypeRuleFactory _rootFactory;
	    private readonly IJsonSchemaCacheProvider _cache;

		public DefaultJsonSchemaFactory(ObjectReferenceTypeRuleFactory rootFactory, IJsonSchemaCacheProvider cache)
		{
			_rootFactory = rootFactory;
			_cache = cache;
		}

	    public IJsonSchema GetSchema(ParseTree jsonSchemaAst, string schemaHash)
        {
            var schema = _cache.Get(schemaHash);
            if (null != schema)
                return schema;

			dynamic dynamicDict = jsonSchemaAst.AsDynamicDictionary();

            //var version = GetSchemaVersion(jsonSchemaAst.Root);
            //var propertyRuleFactories = _propertyRuleFactories.Where(x => x.SupportedSchemaVersions.Contains(version));
			var allowAdditionalProperties = dynamicDict.allowAdditionalProperties == true;
           
            var rules = _rootFactory.GetValidatorRules(dynamicDict);

            var requiredProperties = new string[]{};

            schema = new JsonSchema(rules, allowAdditionalProperties, requiredProperties);
            _cache.Set(schemaHash, schema);

            return schema;
        }

        private JsonSchema.SchemaVersion GetSchemaVersion(ParseTreeNode rootNode)
        {
            var schemaIdentifierProperty = rootNode.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "$schema"));

            if (null == schemaIdentifierProperty || schemaIdentifierProperty.ChildNodes.Count != 2 || !_schemaVersionMap.ContainsKey(schemaIdentifierProperty.ChildNodes[1].Token.ValueString))
            {
                //Revert back to schema version 4 if no identifier is used
                return JsonSchema.SchemaVersion.V4;
            }
            
            var versionIdentifier = schemaIdentifierProperty.ChildNodes[1].Token.ValueString;
            return _schemaVersionMap[versionIdentifier];
        }

        private bool GetAdditionalPropertiesAllowedFlag(ParseTreeNode rootNode)
        {
            return false;
            /*
            var additionalPropertiesProperty = rootNode.ChildNodes.FirstOrDefault(x => x.ChildNodes.Any(y => y.Token.ValueString == "additionalProperties"));

            if (null == additionalPropertiesProperty || additionalPropertiesProperty.ChildNodes.Count != 2)
            {
                //If nothing else is stated, then allow additional propertires
                return true;
            }

            var additionalPropertiesStringvalue = additionalPropertiesProperty.ChildNodes[1].Token.ValueString;
            bool allowAdditionalProperties;
            if (!Boolean.TryParse(additionalPropertiesStringvalue, out allowAdditionalProperties))
            {
                throw new ArgumentException(String.Format("The value of additionalProperties needs to be boolean. Unable to parse '{0}' as a boolean.", additionalPropertiesStringvalue));
            }

            return allowAdditionalProperties;
            */
        }
    }
}
