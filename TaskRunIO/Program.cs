using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace TaskRunIO
{
    [InProcess]
    public class TaskRunIOBenchmark
    {
        // Change to full path.
        public static readonly string ImageName = Path.GetFullPath(@"../../../IMG_20180905_220918");

        public const string ImageExt = ".jpg";

        public static readonly string ImagePath = ImageName + ImageExt;

        public static int Amount { get; set; } = 2;

        private Task GetTask(Func<Task> action) => Task.WhenAll
            (
                Enumerable
                    .Repeat(action, Amount)
                    .Select(it => action())
                    .ToArray()
            );

        [Benchmark]
        public Task AsyncAwaitCopyDelete()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = await Task.Run(() => new Bitmap(ImagePath));
                await Task.Run(() => image.Save(name));
                await Task.Run(() => File.Delete(name));
            });
        }

        [Benchmark]
        public Task AsyncAwaitFalseCopyDelete()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = await Task.Run(() => new Bitmap(ImagePath)).ConfigureAwait(false);
                await Task.Run(() => image.Save(name)).ConfigureAwait(false);
                await Task.Run(() => File.Delete(name)).ConfigureAwait(false);
            });
        }

        [Benchmark]
        public Task SyncCopyDelete()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = new Bitmap(ImagePath);
                image.Save(name);
                File.Delete(name);
            });
        }
    }

    static class Program
    {
        public static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter operations amount.");
                var line = Console.ReadLine();
                if (int.TryParse(line, out var amount))
                {
                    TaskRunIOBenchmark.Amount = amount;
                    Console.WriteLine(TaskRunIOBenchmark.ImagePath);
                    BenchmarkRunner.Run<TaskRunIOBenchmark>();
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Invalid Int32 format.");
                }
            }
        }
    }
}
