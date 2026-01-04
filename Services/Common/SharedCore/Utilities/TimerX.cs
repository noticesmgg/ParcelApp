using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCore.Utilities
{
    public static class TimerX
    {
        public static T Measure<T>(Func<T> func, out TimeSpan duration)
        {
            var sw = Stopwatch.StartNew();
            T result = func();
            sw.Stop();
            duration = sw.Elapsed;
            return result;
        }
    }
}
