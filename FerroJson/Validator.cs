using FerroJson.Bootstrapper;

namespace FerroJson
{
    public class Validator
    {
        public bool Validate()
        {
            var bootStrapper = BootstrapperLocator.Bootstrapper;

            var validator = bootStrapper.GetValidatorLocator().GetValidator();

            return validator.Validate();
        }
    }
}
