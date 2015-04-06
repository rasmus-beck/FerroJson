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

        protected override IReferenceTypeRuleFactoryLocator GetReferenceTypeRuleFactoryLocator(TinyIoCContainer container)
		{
			return container.Resolve<IReferenceTypeRuleFactoryLocator>();
		}

        protected override void RegisterValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> validatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IValidatorRuleFactory), validatorRuleFactoriesTypes);
        }

	    protected override void RegisterReferenceTypeRuleFactories(TinyIoCContainer container, IEnumerable<Type> validatorRuleFactoriesTypes)
		{
			container.RegisterMultiple(typeof(IReferenceTypeRuleFactory), validatorRuleFactoriesTypes);
		}

		protected override void RegisterReferenceTypeRuleFactoryLocator(TinyIoCContainer container, Type objectTypeFactoryLocatorType)
		{
			container.Register(typeof(IReferenceTypeRuleFactoryLocator), objectTypeFactoryLocatorType).AsSingleton();
		}

        protected override IJsonSchemaFactory GetJsonSchemaFactory(TinyIoCContainer container)
        {
            return container.Resolve<IJsonSchemaFactory>();
        }

        protected override void RegisterJsonSchemaFactory(TinyIoCContainer container, Type jsonSchemaFactoryType)
        {
            container.Register(typeof(IJsonSchemaFactory), jsonSchemaFactoryType).AsSingleton();
        }

        protected override void RegisterJsonSchemaCacheProvider(TinyIoCContainer container, Type objectTypeJsonSchemaCacheProvider)
        {
            container.Register(typeof(IJsonSchemaCacheProvider), objectTypeJsonSchemaCacheProvider).AsSingleton();
        }
    }
}
