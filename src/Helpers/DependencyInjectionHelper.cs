using Microsoft.Extensions.DependencyInjection;

namespace Automato.Tasks.Helpers
{
    public static class DependencyInjectionHelper
    {
        private static readonly ServiceCollection Collection = new ServiceCollection();

        public static void RegisterDependency<T, TU>()
        {
            Collection.AddScoped(typeof(T), typeof(TU)).BuildServiceProvider();
        }

        public static T InjectDependency<T>()
        {
            return Collection.BuildServiceProvider().GetService<T>();
        }
    }
}