using System.IO;

namespace Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ToByteArray(this Stream stream)
        {
            int blockSize = 16384;
            return stream.ToByteArray(blockSize);
        }

        public static byte[] ToByteArray(this Stream stream, int blockSize, bool considerMemoryStream = true)
        {
            if (stream is MemoryStream && considerMemoryStream)
            {
                return (stream as MemoryStream).ToArray();
            }

            int num = (int)stream.Length;
            byte[] array = new byte[num];
            int num2 = 0;
            int num3 = 0;
            if (num < blockSize)
            {
                blockSize = num;
            }

            while ((num2 = stream.Read(array, num3, blockSize)) > 0 && (num3 += num2) < num)
            {
                if (num - num3 < blockSize)
                {
                    blockSize = num - num3;
                }
            }

            return array;
        }
    }
}