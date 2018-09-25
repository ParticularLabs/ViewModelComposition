namespace Particular.ViewModelComposition.AspNetCore
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static void AddViewModelComposition(this IServiceCollection services) =>
            AddViewModelComposition(services, "*ViewModelComposition*.dll");

        public static void AddViewModelComposition(this IServiceCollection services, string assemblySearchPattern)
        {
            var types = new List<Type>();

            foreach (var assemblyPath in Directory.GetFiles(AppContext.BaseDirectory, assemblySearchPattern))
            {
                var assemblyTypes = Assembly.LoadFrom(assemblyPath)
                    .GetTypes()
                    .Where(type => !type.GetTypeInfo().IsAbstract && typeof(IRouteInterceptor).IsAssignableFrom(type));

                types.AddRange(assemblyTypes);
            }

            foreach (var type in types)
            {
                services.AddSingleton(typeof(IRouteInterceptor), type);
            }
        }
    }
}
