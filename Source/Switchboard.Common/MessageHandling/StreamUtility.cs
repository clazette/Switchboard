using System;
using System.IO;

namespace Switchboard.Common.MessageHandling
{
    /// <summary>
    /// Utility class for stream operations
    /// </summary>
    public static class StreamUtility
    {
        /// <summary>
        /// Reads the stream fully until there is no more data and then returns the byte array.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>A <see cref="byte"/><see cref="System.Array"/></returns>
        /// <remarks>This came from Jon Skeet. Thanks Jon!</remarks>
        public static byte[] ReadFully(Stream input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
