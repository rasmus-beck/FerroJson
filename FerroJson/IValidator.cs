using Irony.Parsing;

namespace FerroJson
{
    public interface IValidator
    {
        bool CanValidate(ParseTree jsonSchema);
        bool Validate(ParseTree jsonDoc, ParseTree jsonSchema);
    }
}
