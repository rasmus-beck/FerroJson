using System;
using System.Collections.Generic;
using FerroJson.RuleFactories;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IJsonSchemaFactory GetJsonSchemaFactory();

        IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator();

		IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator();
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

	    public IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator()
	    {
		    return GetReferenceTypeRuleFactoryLocator(ApplicationContainer);
	    }

	    protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator(TContainer container);

		protected abstract IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator(TContainer container);

        protected abstract void RegisterValidatorRuleFactories(TContainer container, IEnumerable<Type> validatorRuleFactoriesTypes);

        protected abstract void RegisterValidatorRuleFactoryLocator(TContainer container, Type objectTypeFactoryLocatorType);

		protected abstract void RegisterReferenceTypeRuleFactories(TContainer container, IEnumerable<Type> validatorRuleFactoriesTypes);

		protected abstract void RegisterReferenceTypeRuleFactoryLocator(TContainer container, Type objectTypeFactoryLocatorType);

        protected abstract void RegisterJsonSchemaCacheProvider(TContainer container, Type objectTypeJsonSchemaCacheProvider);

        protected abstract IJsonSchemaFactory GetJsonSchemaFactory(TContainer container);

        protected abstract void RegisterJsonSchemaFactory(TContainer container, Type jsonSchemaFactoryType);
        
        protected virtual void ConfigureApplicationContainer(TContainer container) {}

        protected virtual IEnumerable<Type> ValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IValidatorRuleFactory>(); }
        }

		protected virtual IEnumerable<Type> ReferenceTypeRuleFactories
		{
			get { return AppDomainScanner.Types<IReferenceTypeRuleFactory>(); }
		}

        protected virtual Type JsonSchemaFactory
        {
            get { return typeof(DefaultJsonSchemaFactory); }
        }

        protected virtual Type JsonSchemaCacheProvider
        {
            get { return typeof(DefaultJsonSchemaCacheProvider); }
        }

        protected virtual Type ValidatorRuleFactoryLocator
        {
            get { return typeof(DefaultValidatorRuleFactoryLocator); }
        }

	    protected virtual Type ReferenceTypeRuleFactoryLocator
	    {
			get { return typeof(DefaultReferenceTypeRuleFactoryLocator); }
	    }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterValidatorRuleFactories(ApplicationContainer, ValidatorRuleFactories);
            RegisterValidatorRuleFactoryLocator(ApplicationContainer, ValidatorRuleFactoryLocator);
			RegisterReferenceTypeRuleFactories(ApplicationContainer, ReferenceTypeRuleFactories);
			RegisterReferenceTypeRuleFactoryLocator(ApplicationContainer, ReferenceTypeRuleFactoryLocator);
            RegisterJsonSchemaFactory(ApplicationContainer, JsonSchemaFactory);
            RegisterJsonSchemaCacheProvider(ApplicationContainer, JsonSchemaCacheProvider);
            
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
