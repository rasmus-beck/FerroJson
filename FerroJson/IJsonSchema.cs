using System;
using System.Collections.Generic;
using Irony.Parsing;

namespace FerroJson
{
    public interface IJsonSchema
    {
        bool TryValidate(ParseTree jsonDoc, out IEnumerable<string> validationErrors);
    }

    public interface IJsonSchemaProperty
    {
        IEnumerable<Func<object, bool>> Rules { get; }
    }
}
