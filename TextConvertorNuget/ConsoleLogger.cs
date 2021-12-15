using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextConvertorNuget
{
    public class ConsoleLogger : ILogger
    {
        public void Write(LogEvent logEvent)
        {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.Console()
               .CreateLogger();
            Log.Write(logEvent);
        }
    }
}
