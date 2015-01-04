using System;
using System.Collections.Generic;
using FerroJson.ObjectTypeFactories;
using FerroJson.PropertyRuleFactories;

namespace FerroJson.Bootstrapper
{
    public interface IBootstrapper
    {
        IJsonSchemaFactory GetJsonSchemaFactory();

        IObjectTypeFactoryLocator GetObjectTypeFactoryLocator();
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

        public IObjectTypeFactoryLocator GetObjectTypeFactoryLocator()
        {
            return GetApplicationContainer(ApplicationContainer);
        }

        protected TContainer ApplicationContainer { get; set; }

        protected abstract TContainer GetApplicationContainer();

        protected abstract IObjectTypeFactoryLocator GetApplicationContainer(TContainer container);

        protected abstract void RegisterPropertyValidatorRuleFactories(TContainer container, IEnumerable<Type> propertyValidatorRuleFactoriesTypes);
        
        protected abstract void RegisterObjectValidatorRuleFactories(TContainer container, IEnumerable<Type> objectValidatorRuleFactoriesTypes);

        protected abstract void RegisterObjectTypeFactoryLocator(TContainer container, Type objectTypeFactoryLocatorType);

        protected abstract IJsonSchemaFactory GetJsonSchemaFactory(TContainer container);

        protected abstract void RegisterJsonSchemaFactory(TContainer container, Type jsonSchemaFactoryType);
        
        protected virtual void ConfigureApplicationContainer(TContainer container) {}

        protected virtual IEnumerable<Type> PropertyValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IPropertyValidatorRuleFactory>(); }
        }

        protected virtual IEnumerable<Type> ObjectValidatorRuleFactories
        {
            get { return AppDomainScanner.Types<IObjectTypeFactory>(); }
        }

        protected virtual Type JsonSchemaFactory
        {
            get { return typeof(DefaultJsonSchemaFactory); }
        }

        protected virtual Type ObjectTypeFactoryLocator
        {
            get { return typeof(DefaultObjectTypeFactoryLocator); }
        }

        private void Initialize()
        {
            ApplicationContainer = GetApplicationContainer();
            RegisterPropertyValidatorRuleFactories(ApplicationContainer, PropertyValidatorRuleFactories);
            RegisterObjectValidatorRuleFactories(ApplicationContainer, ObjectValidatorRuleFactories);
            RegisterObjectTypeFactoryLocator(ApplicationContainer, ObjectTypeFactoryLocator);
            RegisterJsonSchemaFactory(ApplicationContainer, JsonSchemaFactory);
            
            ConfigureApplicationContainer(ApplicationContainer);
            _isInitialized = true;
        }
    }
}
