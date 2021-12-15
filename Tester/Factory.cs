using System;
using System.Collections.Generic;
using Autofac;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    public static class Factory
    {
        private static IContainer container = ContainerConfig.Configure();
        public static T GetOfType<T>() where T : notnull
        {
            using(var scope = container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }
    }
}
