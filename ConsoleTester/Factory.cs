using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextConvertorNuget;

namespace Tester
{
    public static class Factory
    {
        public static string method = Converter.PARALLEL; 

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
