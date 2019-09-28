using Microsoft.Extensions.DependencyInjection;
using ProjectSanitizer.Base.Services.Interfaces;
using ProjectSanitizer.Base.Services;
using System.Linq;
using System;
using ProjectSanitizer.Services.Interfaces;

namespace ProjectSanitizer.Base
{
    public static class DIRegistrar
    {
        public delegate IServiceCollection RegisterTypesDelegate(IServiceCollection serviceCollection);
        public static RegisterTypesDelegate RegisterTypes = new RegisterTypesDelegate(RegisterStandardServices);

        private static ServiceProvider _serviceProvider;

        private static ServiceProvider GetOrCreateServiceProvider()
        {
            if(_serviceProvider==null)
            {
                IServiceCollection serviceCollection = new ServiceCollection();
                serviceCollection = RegisterTypes.Invoke(serviceCollection);
               _serviceProvider = serviceCollection.BuildServiceProvider(); 
            }

            return _serviceProvider;
        }

        private static IServiceCollection RegisterStandardServices(IServiceCollection serviceCollection)
        {
            return serviceCollection
               .AddSingleton<IProjectReader, ProjectReader>()
               .AddSingleton<ISolutionReader, SolutionReader>()
               .AddSingleton<IProjectGraphBuilder, ProjectGraphBuilder>()
               .AddSingleton<INugetReferenceReader, NugetReferenceReader>()
               .AddImplementationsOf<IProblemDetector>()
               .AddSingleton<ProblemDetector>();
        }

        public static IServiceCollection AddImplementationsOf<T>(this IServiceCollection serviceCollection)
        {
            var assembly = typeof(T).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if(typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract) 
                    serviceCollection.AddSingleton(typeof(T), type);  
            }

            return serviceCollection.AddSingleton<T[]>(sp => sp.GetServices<T>().ToArray());
        }

        public static T GetInstance<T>() where T:class
        {
            var resolved = GetOrCreateServiceProvider().GetService<T>();
            if (resolved == null)
                throw new System.Exception("There is no implementation registered for type " + typeof(T).Name);
            return resolved;
        }

        public static T[] GetImplementations<T>()
        {
            return GetOrCreateServiceProvider().GetServices<T>().ToArray();
        }
    }
}
