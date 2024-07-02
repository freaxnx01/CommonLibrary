using System.IO;
using System.IO.Compression;

namespace Library.Misc
{
    public class Compression
    {
        public static byte[] Decompress(byte[] compressedByteArray)
        {
            using (GZipStream stream = new GZipStream(new MemoryStream(compressedByteArray), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }
    }
}
