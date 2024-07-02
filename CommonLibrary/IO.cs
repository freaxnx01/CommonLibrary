using Extensions;
using System.IO;
using System.Threading.Tasks;

namespace Library
{
    public class IO
    {
        private static async Task WriteStreamAsync(string path, Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            await WriteAllBytesAsync(path, stream.ToByteArray());
        }

        private static async Task WriteAllBytesAsync(string path, byte[] bytes)
        {
            using (FileStream sourceStream = new(path,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(bytes, 0, bytes.Length);
            };
        }
    }
}
