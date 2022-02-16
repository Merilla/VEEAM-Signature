using System;

namespace VEEAM_Signature
{
    /// <summary>
    /// The thread worker event args about block.
    /// </summary>
    public class ThreadWorkerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThreadWorkerEventArgs"/> class.
        /// </summary> <param name="blockNumber"> The block number. </param>
        /// <param name="hashValue"> The hash string value. </param>
        public ThreadWorkerEventArgs(int blockNumber, string hashValue)
        {
            this.BlockNumber = blockNumber;
            this.HashValue = hashValue;
        }

        /// <summary>
        /// Gets the block number.
        /// </summary>
        public int BlockNumber { get; }

        /// <summary>
        /// Gets the hash string value.
        /// </summary>
        public string HashValue { get; }
    }
}
