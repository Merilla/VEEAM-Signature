using System.IO;
using System.Threading;

namespace VEEAM_Signature
{
    /// <summary>
    /// The static class that reads a file, creates blocks and gives them to threads.
    /// </summary>
    public static class FileStreamer
    {
        /// <summary>
        /// The file stream of the specified file.
        /// </summary>
        private static FileStream file;

        /// <summary>
        /// Number of bytes in one block.
        /// </summary>
        private static int bytesCount;

        /// <summary>
        /// The current block of bytes.
        /// </summary>
        private static byte[] buffer;

        /// <summary>
        /// The number of the current block.
        /// </summary>
        private static int blockNumber;

        /// <summary>
        /// The file stream and number of bytes initialization.
        /// </summary>
        /// <param name="pathToFile"> The path to the specified file. </param>
        /// <param name="blockSize"> The block bytes size. </param>
        public static void Initialization(string pathToFile, int blockSize)
        {
            file = new FileStream(pathToFile, FileMode.Open);
            bytesCount = blockSize;
        }

        /// <summary>
        /// Get next block of bytes from the file.
        /// </summary>
        /// <returns> The current <see cref="Block"/>. </returns>
        public static Block NextBlock()
        {
            // Lock the file to read the next block in the correct sequence.
            lock (file)
            {
                // If that was the last block, then stop reading.
                if (!file.CanRead)
                {
                    return null;
                }

                // Get the size of the current block and reinitialize the buffer.
                bytesCount = GetBytesCount();
                buffer = new byte[bytesCount];

                // Check if we achieve the end of the file and read the next block to buffer.
                if (file.Read(buffer, 0, bytesCount) != 0)
                {
                    // Returns a new file block with the current buffer and incremented block number.
                    return new Block(Interlocked.Increment(ref blockNumber), buffer);
                }

                // Close the file when we achieved the EOF.
                file.Close();
                return null;
            }
        }

        /// <summary>
        /// Get current bytes size of the block.
        /// Checks that the last block gets a correct size.
        /// </summary>
        /// <returns> The <see cref="int"/>.</returns>
        private static int GetBytesCount() =>
            bytesCount = bytesCount < file.Length - file.Position ? bytesCount : (int)(file.Length - file.Position);
    }
}
