using ClockWidget.Models.Initialization;
using DryIoc;
using Prism.Container.DryIoc;
using Prism.Ioc;

namespace ClockWidget.Models
{
    public static class ContainerRegistryExtensions
    {
        public static IContainerRegistry RegisterSingletonWithInitializable<TInterface, TImplementation>(this IContainerRegistry containerRegistry, int priority = 0, bool shouldInitialize = true)
            where TImplementation : class, TInterface, IAsyncInitializable
        {
            containerRegistry.RegisterSingleton<TImplementation>();

            var container = containerRegistry.GetContainer();

            var instance = container.Resolve<TImplementation>();

            containerRegistry.RegisterInstance<IAsyncInitializable>(instance);
            containerRegistry.RegisterInstance<TInterface>(instance);

            containerRegistry.RegisterInstance(new InitializableEntry(instance, priority, shouldInitialize));

            return containerRegistry;
        }
    }
}
