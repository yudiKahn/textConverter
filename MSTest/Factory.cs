using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextConvertorNuget;

namespace MSTest
{
    internal class Factory
    {
        public static string method = Converter.RECURSION;

        public static T GetOfType<T>() where T : notnull
        {
            var container = config();
            using (var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
        private static IContainer config()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(Assembly.Load(nameof(TextConvertorNuget)))
                .Where(t => t.Namespace.Contains(method))
                .As(t => Assembly
                            .Load(nameof(TextConvertorNuget)).DefinedTypes
                            .Where(i => i.Namespace.Contains("Interfaces") && i.Name == "I" + t.Name)
                );

            builder.RegisterType<Converter>().As<IConverter>();
            //builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }
    }
}
