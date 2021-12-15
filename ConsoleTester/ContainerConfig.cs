using Autofac;
using TextConvertorNuget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Serilog;
using Interfaces = TextConvertorNuget.Interfaces;
using Recursion = TextConvertorNuget.Recursion;
using Parallel = TextConvertorNuget.Parallel;

namespace Tester
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

           /* builder
                .RegisterAssemblyTypes(Assembly.Load(nameof(TextConvertorNuget)))
                .Where(t => t.Namespace.Contains("Recursion"))
                .As(t => Assembly
                            .Load(nameof(TextConvertorNuget)).DefinedTypes
                            .Where(i => i.Namespace.Contains("Interfaces") && i.Name == "I" + t.Name)
                );*/
            builder.RegisterType<Recursion.JsonDeserializer>().As<Interfaces.IJsonDeserializer>();
            builder.RegisterType<Recursion.JsonSerializer>().As<Interfaces.IJsonSerializer>();
            builder.RegisterType<Recursion.XmlSerializer>().As<Interfaces.IXmlSerializer>();
            builder.RegisterType<Recursion.XmlDeserializer>().As<Interfaces.IXmlDeserializer>();

            builder.RegisterType<Converter>().As<IConverter>();
            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }

        public static void IoCContainer(Action<ILifetimeScope> action)
        {
            var container = Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                action(scope);
            }
        }
    }
}
