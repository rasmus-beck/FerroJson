using System;
using System.Collections.Generic;
using FerroJson.RuleFactories;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IJsonSchemaFactory GetJsonSchemaFactory();

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

        public IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator()
	    {
		    return GetReferenceTypeRuleFactoryLocator(ApplicationContainer);
	    }

	    protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator(TContainer container);

        protected abstract void RegisterValidatorRuleFactories(TContainer container, IEnumerable<Type> validatorRuleFactoriesTypes);

        protected abstract void RegisterReferenceTypeRuleFactories(TContainer container, IEnumerable<Type> referenceTypeRuleFactoriesTypes);

		protected abstract void RegisterReferenceTypeRuleFactoryLocator(TContainer container, Type referenceTypeRuleFactoryLocatorType);

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

	    protected virtual Type ReferenceTypeRuleFactoryLocator
	    {
			get { return typeof(DefaultReferenceTypeRuleFactoryLocator); }
	    }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterValidatorRuleFactories(ApplicationContainer, ValidatorRuleFactories);
			RegisterReferenceTypeRuleFactories(ApplicationContainer, ReferenceTypeRuleFactories);
			RegisterReferenceTypeRuleFactoryLocator(ApplicationContainer, ReferenceTypeRuleFactoryLocator);
            RegisterJsonSchemaFactory(ApplicationContainer, JsonSchemaFactory);
            RegisterJsonSchemaCacheProvider(ApplicationContainer, JsonSchemaCacheProvider);
            
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
