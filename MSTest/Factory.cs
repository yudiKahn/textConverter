using Autofac;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TextConvertorNuget;

namespace MSTest
{
    public static class ContainerConfig
    {
        public static string method = Converter.RECURSION;
        public static IContainer Configure()
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
            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }
    }
    public static class Factory
    {
        public static string method = Converter.RECURSION;

        public static T GetOfType<T>() where T : notnull
        {
            ContainerConfig.method = method;
            var container = ContainerConfig.Configure();
            using (var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
    }
}
