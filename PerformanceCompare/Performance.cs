using System.Diagnostics;

namespace PerformanceCompare
{
    public static class Performance
    {
        public static int N { get; set; } = 100;
        public static IEnumerable<Info> Compare(params Action[] actions)
        {
            List<Info> infos = new List<Info>();
            if (actions == null || actions.Length == 0) throw new ArgumentNullException();
            int method = 1;
            foreach (var action in actions)
            {
                try
                {
                    var ts = new Stopwatch();
                    ts.Start();
                    for(var i = 0; i < N; i++)
                        action();
                    ts.Stop();
                    infos.Add(new Info(
                        name: $"Method {method}",
                        time: (ts.ElapsedMilliseconds/N).ToString()));
                }
                catch
                {
                    infos.Add(new Info($"Method {method} crashed", "0"));
                } finally
                {
                    method++;
                }
            }
            return infos.ToArray();
        }

        public static IEnumerable<Info> Compare(params Func<string>[] funcs)
        {
            List<Info> infos = new();
            if (funcs == null || funcs.Length == 0) throw new ArgumentNullException();
            string funcName = "";
            foreach (var func in funcs)
            {
                try
                {
                    var ts = new Stopwatch();
                    ts.Start();
                    for(var i = 0; i < N; i++)
                        funcName = func();
                    ts.Stop();
                    infos.Add(new Info(
                        name: $"Method {funcName}",
                        time: (ts.ElapsedMilliseconds / N).ToString()));
                }
                catch(Exception ex)
                {
                    infos.Add(new Info($"Method {funcName} crashed. {ex.Message}", "0"));
                }
            }
            return infos.ToArray();
        }
    }
}