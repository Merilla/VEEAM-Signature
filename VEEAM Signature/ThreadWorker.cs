using System;
using System.Security.Cryptography;

namespace VEEAM_Signature
{
    /// <summary>
    /// The thread worker class that gets blocks of bytes from the specified file and computes a hash for them.
    /// </summary>
    public class ThreadWorker
    {
        /// <summary>
        /// The signature calculated event.
        /// </summary>
        public event EventHandler<ThreadWorkerEventArgs> SignatureCalculated;

        /// <summary>
        /// The thread completed event.
        /// </summary>
        public event EventHandler ThreadCompleted;

        /// <summary>
        /// Start the process of getting blocks and computing the hash.
        /// </summary>
        public void Run()
        {
            try
            {
                Block block;

                // Get blocks until EOF is reached.
                while ((block = FileStreamer.NextBlock()) != null)
                {
                    using (var hasher = SHA256.Create())
                    {
                        var hash = BitConverter.ToString(hasher.ComputeHash(block.Source));

                        // Raise the print result event.
                        this.SignatureCalculated?.Invoke(this, new ThreadWorkerEventArgs(block.Number, hash));
                    }
                }

                // Raise event about EOF reaching and the end of the current thread for multi-threading.
                this.ThreadCompleted?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exc)
            {
                // Exception handling per thread.
                Console.WriteLine("An exception: {0}\nStackTrace: {1}", exc.Message, exc.StackTrace);
            }
        }
    }
}
