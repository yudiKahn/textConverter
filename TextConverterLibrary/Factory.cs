using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TextConverterLibrary
{
    public class Factory
    {
        public static string Method = Converter.PARALLEL;
        public static T GetOfType<T>() where T : notnull
        {
            var container = GetContainer();
            using(var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(Assembly.Load(nameof(TextConverterLibrary)))
                .Where(t => t.Namespace.Contains(Method))
                .As(t => Assembly
                            .Load(nameof(TextConverterLibrary)).DefinedTypes
                            .Where(i => i.Namespace.Contains("Interfaces") && i.Name == "I" + t.Name)
                );

            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }
    }
}
