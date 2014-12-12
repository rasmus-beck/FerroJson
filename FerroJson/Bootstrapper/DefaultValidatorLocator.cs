namespace FerroJson.Bootstrapper
{
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultValidatorLocator : IValidatorLocator
    {
        private readonly IEnumerable<IValidator> _validators;

        public DefaultValidatorLocator(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public IValidator GetValidator()
        {
            return _validators.FirstOrDefault(v => v.CanValidate());
        }
    }
}
