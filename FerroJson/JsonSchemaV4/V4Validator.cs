namespace FerroJson.JsonSchemaV4
{
    public class V4Validator : IValidator
    {
        public bool CanValidate()
        {
            return true;
        }

        public bool Validate()
        {
            return false;
        }
    }
}
