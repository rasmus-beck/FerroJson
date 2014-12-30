using System;
using System.Collections.Generic;
using System.Linq;
using FerroJson.ObjectRuleFactories;
using FerroJson.PropertyRuleFactories;
using Irony.Parsing;

namespace FerroJson
{
    public interface IJsonSchemaFactory
    {
        IJsonSchema GetSchema(ParseTree jsonSchemaAst);
    }

    public class JsonSchemaFactory : IJsonSchemaFactory
    {
        private readonly Dictionary<string, JsonSchema.SchemaVersion> _schemaVersionMap = new Dictionary <string, JsonSchema.SchemaVersion>
        {
            {"http://json-schema.org/draft-01/schema#", JsonSchema.SchemaVersion.V1},
            {"http://json-schema.org/draft-02/schema#", JsonSchema.SchemaVersion.V2},
            {"http://json-schema.org/draft-03/schema#", JsonSchema.SchemaVersion.V3},
            {"http://json-schema.org/draft-04/schema#", JsonSchema.SchemaVersion.V4}
        };

        private readonly IEnumerable<IPropertyValidatorRuleFactory> _propertyRuleFactories;
        private readonly IEnumerable<IObjectValidatorRuleFactory> _objectRuleFactories;

        public JsonSchemaFactory(IEnumerable<IPropertyValidatorRuleFactory> propertyRuleFactories, IEnumerable<IObjectValidatorRuleFactory> objectRuleFactories)
        {
            _propertyRuleFactories = propertyRuleFactories;
            _objectRuleFactories = objectRuleFactories;
        }

        public IJsonSchema GetSchema(ParseTree jsonSchemaAst)
        {
            //ToDo... check cache first

            var version = GetSchemaVersion(jsonSchemaAst.Root);
            var propertyRuleFactories = _propertyRuleFactories.Where(x => x.SupportedSchemaVersions.Contains(version));
            var allowAdditionalProperties = GetAdditionalPropertiesAllowedFlag(jsonSchemaAst.Root);
            

            var propertyRules = new SortedDictionary<string, IList<Func<object, bool>>>();
            //TODO: Next run through Json schema abstract syntax tree to build the property rules dictionary. 
            //Find a suitable key based on location in document, this should also support arrays

            var requiredProperties = new string[]{};

            return new JsonSchema(propertyRules, allowAdditionalProperties, requiredProperties);
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
        }
    }
}
