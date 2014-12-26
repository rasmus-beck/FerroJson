using System;
using System.Collections.Generic;
using FerroJson.JsonSchemaV4;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IValidatorLocator GetValidatorLocator();
    }

    public abstract class Bootstrapper<TContainer> : IBootstrapper
    {
        private bool _isInitialized;

        public IValidatorLocator GetValidatorLocator()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            return GetValidatorLocator(ApplicationContainer);
        }

        protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IValidatorLocator GetValidatorLocator(TContainer container);

        protected abstract void RegisterValidators(TContainer container, IEnumerable<Type> validatorTypes);

        protected abstract void RegisterValidatorV4Rules(TContainer container, IEnumerable<Type> validatorV4RuleTypes);
        
        protected abstract void RegisterValidatorLocator(TContainer container, Type validatorLocatorType);

        protected virtual void ConfigureApplicationContainer(TContainer container)
        {
        }

        protected virtual IEnumerable<Type> Validators
        {
            get { return AppDomainScanner.Types<IValidator>(); }
        }

        protected virtual IEnumerable<Type> ValidatorV4Rules
        {
            get { return AppDomainScanner.Types<IJsonSchemaV4ValidatorRuleFactory>(); }
        }

        protected virtual Type ValidatorLocator
        {
            get { return typeof(DefaultValidatorLocator); }
        }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterValidators(ApplicationContainer, Validators);
            RegisterValidatorV4Rules(ApplicationContainer, ValidatorV4Rules);
            RegisterValidatorLocator(ApplicationContainer, ValidatorLocator);
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
