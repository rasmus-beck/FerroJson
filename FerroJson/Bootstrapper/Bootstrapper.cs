using System;
using System.Collections.Generic;
using FerroJson.RuleFactories;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IJsonSchemaFactory GetJsonSchemaFactory();

        IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator();
    }

    public abstract class Bootstrapper<TContainer> : IBootstrapper
    {
        private bool _isInitialized;

        public IJsonSchemaFactory GetJsonSchemaFactory()
        {
            if (!_isInitialized)
            {
                Initialize();
            }

            return GetJsonSchemaFactory(ApplicationContainer);
        }

        public IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator()
        {
            return GetValidatorRuleFactoryLocator(ApplicationContainer);
        }

        protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator(TContainer container);

        protected abstract void RegisterValidatorRuleFactories(TContainer container, IEnumerable<Type> validatorRuleFactoriesTypes);

        protected abstract void RegisterValidatorRuleFactoryLocator(TContainer container, Type objectTypeFactoryLocatorType);

        protected abstract IJsonSchemaFactory GetJsonSchemaFactory(TContainer container);

        protected abstract void RegisterJsonSchemaFactory(TContainer container, Type jsonSchemaFactoryType);
        
        protected virtual void ConfigureApplicationContainer(TContainer container) {}

        protected virtual IEnumerable<Type> ValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IValidatorRuleFactory>(); }
        }

        protected virtual Type JsonSchemaFactory
        {
            get { return typeof(DefaultJsonSchemaFactory); }
        }

        protected virtual Type ValidatorRuleFactoryLocator
        {
            get { return typeof(DefaultValidatorRuleFactoryLocator); }
        }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterValidatorRuleFactories(ApplicationContainer, ValidatorRuleFactories);
            RegisterValidatorRuleFactoryLocator(ApplicationContainer, ValidatorRuleFactoryLocator);
            RegisterJsonSchemaFactory(ApplicationContainer, JsonSchemaFactory);
            
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
