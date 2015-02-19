namespace FerroJson
{
    public interface IPropertyValidationResult
    {
        string Error { get; set; }
        string ErrorCode { get; set; }
    }

    public class PropertyValidationResult : IPropertyValidationResult
    {
        public string Error { get; set; }
        public string ErrorCode { get; set; }
    }
}
