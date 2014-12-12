using System;
using System.Collections.Generic;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IEnumerable<IValidator> GetValidators();
        IValidatorLocator GetValidatorLocator();
    }

    public abstract class Bootstrapper<TContainer> : IBootstrapper
    {
        private bool isInitialized;

        public IEnumerable<IValidator> GetValidators()
        {
            if (!this.isInitialized)
            {
                this.Initialize();
            }

            return this.GetValidators(this.ApplicationContainer);
        }

        public IValidatorLocator GetValidatorLocator()
        {
            if (!this.isInitialized)
            {
                this.Initialize();
            }

            return this.GetValidatorLocator(this.ApplicationContainer);
        }

        protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IEnumerable<IValidator> GetValidators(TContainer container);

        protected abstract IValidatorLocator GetValidatorLocator(TContainer container);

        protected abstract void RegisterValidators(TContainer container, IEnumerable<Type> validatorRuleTypes);
        
        protected abstract void RegisterValidatorLocator(TContainer container, IEnumerable<Type> validatorRuleTypes);

        protected virtual void ConfigureApplicationContainer(TContainer container)
        {
        }

        protected virtual IEnumerable<Type> Validators
        {
            get { return AppDomainScanner.Types<IValidator>(); }
        }

        protected virtual IEnumerable<Type> ValidatorLocator
        {
            get { return AppDomainScanner.Types<IValidatorLocator>(); }
        }

        private void Initialize()
        {
            this.ApplicationContainer = this.GetApplicationContainer();
            this.RegisterValidators(this.ApplicationContainer, this.Validators);
            this.RegisterValidatorLocator(this.ApplicationContainer, this.ValidatorLocator);
            this.ConfigureApplicationContainer(this.ApplicationContainer);
            this.isInitialized = true;
        }
    }
}
