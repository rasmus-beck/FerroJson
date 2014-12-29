namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using TinyIoC;
    using ObjectRuleFactories;
    using PropertyRuleFactories;

    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return new TinyIoCContainer();
        }

        protected override void RegisterPropertyValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> propertyValidatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IPropertyValidatorRuleFactory), propertyValidatorRuleFactoriesTypes);
        }

        protected override void RegisterObjectValidatorRuleFactories(TinyIoCContainer container, IEnumerable<Type> objectValidatorRuleFactoriesTypes)
        {
            container.RegisterMultiple(typeof(IObjectValidatorRuleFactory), objectValidatorRuleFactoriesTypes);
        }

        protected override IJsonSchemaFactory GetJsonSchemaFactory(TinyIoCContainer container)
        {
            return container.Resolve<IJsonSchemaFactory>();
        }
    }
}
