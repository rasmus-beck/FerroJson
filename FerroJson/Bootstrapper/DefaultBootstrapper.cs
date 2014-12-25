namespace FerroJson.Bootstrapper
{
    using System;
    using System.Collections.Generic;
    using TinyIoC;
    using JsonSchemaV4;

    public class DefaultBootstrapper : Bootstrapper<TinyIoCContainer>
    {
        protected override TinyIoCContainer GetApplicationContainer()
        {
            return new TinyIoCContainer();
        }

        protected override IValidatorLocator GetValidatorLocator(TinyIoCContainer container)
        {
            return container.Resolve<IValidatorLocator>();
        }

        protected override void RegisterValidators(TinyIoCContainer container, IEnumerable<Type> validatorTypes)
        {
            container.RegisterMultiple(typeof(IValidator), validatorTypes);
        }

        protected override void RegisterValidatorV4Rules(TinyIoCContainer container, IEnumerable<Type> validatorV4RuleTypes)
        {
            container.RegisterMultiple(typeof(IJsonSchemaV4ValidatorRule), validatorV4RuleTypes);
        }

        protected override void RegisterValidatorLocator(TinyIoCContainer container, Type validatorLocatorType)
        {
            container.Register(typeof(IValidatorLocator), validatorLocatorType).AsSingleton();
        }
    }
}
