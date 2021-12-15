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

            var method = new
            {
                Recursion = "Recursion",
                Parallel = "Parallel"
            };
            builder
                .RegisterAssemblyTypes(Assembly.Load(nameof(TextConvertorNuget)))
                .Where(t => t.Namespace.Contains(method.Recursion))
                .As(t => Assembly
                            .Load(nameof(TextConvertorNuget)).DefinedTypes
                            .Where(i => i.Namespace.Contains("Interfaces") && i.Name == "I" + t.Name)
                );
            /*builder.RegisterType<Parallel.JsonDeserializer>().As<Interfaces.IJsonDeserializer>();
            builder.RegisterType<Parallel.JsonSerializer>().As<Interfaces.IJsonSerializer>();
            builder.RegisterType<Parallel.XmlSerializer>().As<Interfaces.IXmlSerializer>();
            builder.RegisterType<Parallel.XmlDeserializer>().As<Interfaces.IXmlDeserializer>();*/

            builder.RegisterType<Converter>().As<IConverter>();
            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }
    }
}
