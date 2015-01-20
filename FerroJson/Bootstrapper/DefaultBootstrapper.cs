namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using TinyIoC;
    using RuleFactories;

    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return new TinyIoCContainer();
        }

        protected override IValidatorRuleFactoryLocator GetValidatorRuleFactoryLocator(TinyIoCContainer container)
        {
            return container.Resolve<IValidatorRuleFactoryLocator>();
        }

        protected override void RegisterValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> validatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IValidatorRuleFactory), validatorRuleFactoriesTypes);
        }

        protected override void RegisterValidatorRuleFactoryLocator(TinyIoCContainer container, Type objectTypeFactoryLocatorType)
        {
            container.Register(typeof(IValidatorRuleFactoryLocator), objectTypeFactoryLocatorType).AsSingleton();
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
