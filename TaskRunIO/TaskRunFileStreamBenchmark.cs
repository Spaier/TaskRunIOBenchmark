using System.IO;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using static TaskRunIO.TestImage;
using static TaskRunIO.BenchmarkConfiguration;

namespace TaskRunIO
{
    [InProcess]
    public class TaskRunFileStreamBenchmark
    {
        private FileStream OpenAsyncFileStream() => new FileStream(
                ImagePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                4096,
                true);

        private FileStream OpenFileStream() => new FileStream(
                ImagePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read);

        private Task<FileStream> OpenAsyncFileStreamAsync() => Task.Run(() => OpenAsyncFileStream());

        private Task<FileStream> OpenFileStreamAsync() => Task.Run(() => OpenFileStream());

        [Benchmark]
        public Task AsyncFileStreamAsync()
        {
            return GetTask(async () =>
            {
                using (var fs = await OpenAsyncFileStreamAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(fs))
                {
                    var text = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            });
        }

        [Benchmark]
        public Task SyncFileStreamAsync()
        {
            return GetTask(async () =>
            {
                using (var fs = await OpenFileStreamAsync().ConfigureAwait(false))
                using (var sr = new StreamReader(fs))
                {
                    var text = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            });
        }

        [Benchmark]
        public Task AsyncFileStreamSync()
        {
            return GetTask(async () =>
            {
                using (var fs = OpenAsyncFileStream())
                using (var sr = new StreamReader(fs))
                {
                    var text = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            });
        }

        [Benchmark]
        public Task SyncFileStreamSync()
        {
            return GetTask(async () =>
            {
                using (var fs = OpenFileStream())
                using (var sr = new StreamReader(fs))
                {
                    var text = await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            });
        }
    }
}
