namespace FerroJson.Bootstrapper
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IValidatorLocator
    {
        IValidator GetValidator();
    }

    public class ValidatorLocator : IValidatorLocator
    {
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorLocator(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }

        public IValidator GetValidator()
        {
            return _validators.FirstOrDefault(v => v.CanValidate());
        }
    }
}
