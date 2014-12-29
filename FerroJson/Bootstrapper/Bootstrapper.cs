using System;
using System.Collections.Generic;
using FerroJson.ObjectRuleFactories;
using FerroJson.PropertyRuleFactories;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IJsonSchemaFactory GetJsonSchemaFactory();
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

        protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract void RegisterPropertyValidatorRuleFactories(TContainer container, IEnumerable<Type> propertyValidatorRuleFactoriesTypes);
        
        protected abstract void RegisterObjectValidatorRuleFactories(TContainer container, IEnumerable<Type> objectValidatorRuleFactoriesTypes);

        protected abstract IJsonSchemaFactory GetJsonSchemaFactory(TContainer container);
        
        protected virtual void ConfigureApplicationContainer(TContainer container) {}

        protected virtual IEnumerable<Type> PropertyValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IPropertyValidatorRuleFactory>(); }
        }

        protected virtual IEnumerable<Type> ObjectValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IObjectValidatorRuleFactory>(); }
        }

        protected virtual Type JsonSchemaFactory
        {
            get { return typeof(IJsonSchemaFactory); }
        }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterPropertyValidatorRuleFactories(ApplicationContainer, PropertyValidatorRuleFactories);
            RegisterObjectValidatorRuleFactories(ApplicationContainer, ObjectValidatorRuleFactories);
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
