using System.Diagnostics;

namespace PerformanceCompare
{
    public static class Performance
    {
        public static IEnumerable<Info> Compare(params Action[] actions)
        {
            List<Info> infos = new List<Info>();
            if (actions == null || actions.Length == 1) throw new ArgumentNullException();
            int method = 1;
            foreach (var action in actions)
            {
                try
                {
                    var ts = new Stopwatch();
                    ts.Start();
                    action();
                    ts.Stop();
                    infos.Add(new Info(
                        name: $"Method {method}",
                        time: ts.ElapsedMilliseconds.ToString()));
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
            List<Info> infos = new List<Info>();
            if (funcs == null || funcs.Length == 1) throw new ArgumentNullException();
            string funcName = "";
            foreach (var func in funcs)
            {
                try
                {
                    var ts = new Stopwatch();
                    ts.Start();
                    funcName = func();
                    ts.Stop();
                    infos.Add(new Info(
                        name: $"Method {funcName}",
                        time: ts.ElapsedMilliseconds.ToString()));
                }
                catch
                {
                    infos.Add(new Info($"Method {funcName} crashed", "0"));
                }
            }
            return infos.ToArray();
        }
    }
}