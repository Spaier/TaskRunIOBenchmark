using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using static TaskRunIO.TestImage;
using static TaskRunIO.BenchmarkConfiguration;

namespace TaskRunIO
{
    [InProcess]
    public class TaskRunIOBenchmark
    {
        [Benchmark]
        public Task AsyncBitmapCopyDelete()
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
        public Task AsyncAwaitFalseBitmapCopyDelete()
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
        public Task AsyncAwaitFalseCopyDelete()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = new Bitmap(ImagePath);
                await Task.Run(() => image.Save(name)).ConfigureAwait(false);
                await Task.Run(() => File.Delete(name)).ConfigureAwait(false);
            });
        }

        [Benchmark]
        public Task AsyncAwaitFalseBitmapCopy()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = await Task.Run(() => new Bitmap(ImagePath)).ConfigureAwait(false);
                await Task.Run(() => image.Save(name)).ConfigureAwait(false);
                File.Delete(name);
            });
        }

        [Benchmark]
        public Task AsyncAwaitFalseBitmapDelete()
        {
            return GetTask(async () =>
            {
                var name = ImageName + Guid.NewGuid() + ImageExt;
                var image = await Task.Run(() => new Bitmap(ImagePath)).ConfigureAwait(false);
                image.Save(name);
                await Task.Run(() => File.Delete(name)).ConfigureAwait(false);
            });
        }

        [Benchmark]
        public Task Sync()
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
}
