namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using TinyIoC;
    using PropertyRuleFactories;
    using ObjectTypeFactories;

    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return new TinyIoCContainer();
        }

        protected override IObjectTypeFactoryLocator GetApplicationContainer(TinyIoCContainer container)
        {
            return container.Resolve<IObjectTypeFactoryLocator>();
        }

        protected override void RegisterPropertyValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> propertyValidatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IPropertyValidatorRuleFactory), propertyValidatorRuleFactoriesTypes);
        }

        protected override void RegisterObjectValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> objectValidatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IObjectTypeFactory), objectValidatorRuleFactoriesTypes);
        }

        protected override void RegisterObjectTypeFactoryLocator(TinyIoCContainer container, Type objectTypeFactoryLocatorType)
        {
            container.Register(typeof(IObjectTypeFactoryLocator), objectTypeFactoryLocatorType).AsSingleton();
        }

        protected override IJsonSchemaFactory GetJsonSchemaFactory(TinyIoCContainer container)
        {
            return container.Resolve<IJsonSchemaFactory>();
        }

        protected override void RegisterJsonSchemaFactory(TinyIoCContainer container, Type jsonSchemaFactoryType)
        {
            container.Register(typeof(IJsonSchemaFactory), jsonSchemaFactoryType).AsSingleton();
        }
    }
}
