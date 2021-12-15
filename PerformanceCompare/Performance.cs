using System.Diagnostics;

namespace PerformanceCompare
{
    public static class Performance
    {
        public static Info[] Compare(params Action[] actions)
        {
            List<Info> infos = new List<Info>();
            if (actions == null || actions.Length == 1) throw new Exception();
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
    }
}