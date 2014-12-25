using Irony.Parsing;

namespace FerroJson.Bootstrapper
{
    public interface IValidatorLocator
    {
        IValidator GetValidator(ParseTree jsonSchema);
    }
}