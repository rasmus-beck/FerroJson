namespace FerroJson.JsonSchemaV4
{
    public interface IJsonSchemaV4ValidatorRule
    {
        bool CanValidate();
        bool Validate();
    }
}