using System;

namespace VEEAM_Signature
{
    /// <summary>
    /// The block of bytes from the specified file.
    /// </summary>
    public class Block
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Block"/> class.
        /// </summary>
        /// <param name="blockNumber"> The number of the current block. </param>
        /// <param name="blockSource"> The array of bytes. </param>
        public Block(int blockNumber, byte[] blockSource)
        {
            this.Number = blockNumber;
            this.Source = new byte[blockSource.Length];

            this.SourceInitialization(blockSource);
        }

        /// <summary>
        /// Gets or sets the number of the block.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the block source.
        /// </summary>
        public byte[] Source { get; set; }

        /// <summary>
        /// The source initialization by copying the buffer from the <see cref="FileStreamer"/> static class.
        /// </summary>
        /// <param name="blockSource"> The current buffer from the file. </param>
        private void SourceInitialization(byte[] blockSource) =>
            Buffer.BlockCopy(blockSource, 0, this.Source, 0, blockSource.Length);
    }
}
