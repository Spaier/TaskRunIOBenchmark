using System;
using System.Linq;
using System.Threading.Tasks;

namespace TaskRunIO
{
    public static class BenchmarkConfiguration
    {
        public static int Amount { get; set; } = 1;

        public static Task GetTask(Func<Task> action) => Task.WhenAll
        (
            Enumerable
                .Repeat(action, Amount)
                .Select(it => action())
                .ToArray()
        );
    }
}
