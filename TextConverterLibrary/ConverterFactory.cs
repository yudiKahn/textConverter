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
    public class ConverterFactory
    {
        public const string Parallel = "Parallel";
        public const string Recursion = "Recursion";
        public static string Method { get; set; } = Parallel;
        public static T GetOfType<T>() where T : notnull
        {
            var container = GetContainer();
            using var scope = container.BeginLifetimeScope();
            return scope.Resolve<T>();
        }
        private static IContainer GetContainer()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyTypes(Assembly.Load(nameof(TextConverterLibrary)))
                .Where(t => t.Namespace.Contains(Method))
                .As(t => t.GetInterfaces()[0]);

            builder.RegisterType<ConsoleLogger>().As<ILogger>();

            return builder.Build();
        }
    }
}
