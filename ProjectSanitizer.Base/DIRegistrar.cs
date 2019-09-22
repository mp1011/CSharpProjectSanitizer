using Microsoft.Extensions.DependencyInjection;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Base.Services;
using System.Linq;

namespace ProjectSanitizer.Base
{
    public static class DIRegistrar
    {
        private static ServiceProvider _serviceProvider;

        static DIRegistrar()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IProjectReader, ProjectReader>()
                .AddSingleton<ISolutionReader, SolutionReader>()
                .AddSingleton<IProjectGraphBuilder, ProjectGraphBuilder>()
                .AddSingleton<INugetReferenceReader, NugetReferenceReader>()
                .AddImplementationsOf<IProblemDetector>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static IServiceCollection AddImplementationsOf<T>(this IServiceCollection serviceCollection)
        {
            var assembly = typeof(T).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if(typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) 
                    serviceCollection.AddSingleton(typeof(T), type);  
            }

            return serviceCollection;
        }

        public static T GetInstance<T>() where T:class
        {
            var resolved = _serviceProvider.GetService<T>();
            if (resolved == null)
                throw new System.Exception("There is no implementation registered for type " + typeof(T).Name);
            return resolved;
        }

        public static T[] GetImplementations<T>()
        {
            return _serviceProvider.GetServices<T>().ToArray();
        }
    }
}
