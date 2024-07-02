using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Library
{
    public static class StreamHelper
    {
        public static string GetAsString(Stream stream)
        {
            StringBuilder sb = new StringBuilder();

            byte[] buffer = new byte[1024];

            int count = 0;

            do
            {
                count = stream.Read(buffer, 0, buffer.Length);

                if (count != 0)
                {
                    sb.Append(Encoding.ASCII.GetString(buffer, 0, count));
                }
            }
            while (count > 0);

            return sb.ToString();
        }
    }
}
