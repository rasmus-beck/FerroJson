namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using TinyIoC;

    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return new TinyIoCContainer();
        }

        protected override IEnumerable<IValidator> GetValidators(TinyIoCContainer container)
        {
            return container.ResolveAll<IValidator>();
        }

        protected override IValidatorLocator GetValidatorLocator(TinyIoCContainer container)
        {
            return container.Resolve<IValidatorLocator>();
        }

        protected override void RegisterValidators(TinyIoCContainer container, IEnumerable<Type> validatorRuleTypes)
        {
            container.RegisterMultiple(typeof(IValidator), validatorRuleTypes);
        }

        protected override void RegisterValidatorLocator(TinyIoCContainer container, Type validatorLocatorType)
        {
            container.Register(typeof(IValidatorLocator), validatorLocatorType).AsSingleton();
        }
    }
}
