using System;
using BenchmarkDotNet.Running;

namespace TaskRunIO
{
    static class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter operations amount.");
                if (!int.TryParse(Console.ReadLine(), out var amount))
                {
                    Console.WriteLine("Invalid Int32 amount format.");
                }

                BenchmarkConfiguration.Amount = amount;

                if (!int.TryParse(Console.ReadLine(), out var benchmarkNumber))
                {
                    Console.WriteLine("Invalid Int32 benchmarkNumber format.");
                }

                switch (benchmarkNumber)
                {
                    case 0:
                        BenchmarkRunner.Run<TaskRunIOBenchmark>();
                        break;
                    case 1:
                        BenchmarkRunner.Run<TaskRunFileStreamBenchmark>();
                        break;
                }
            }
        }
    }
}
