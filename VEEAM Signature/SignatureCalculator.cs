using System;
using System.Threading;

namespace VEEAM_Signature
{
    /// <summary>
    /// The signature calculator class that starts a multi-threaded calculation and prints the result.
    /// </summary>
    public class SignatureCalculator
    {
        /// <summary>
        /// The thread workers, the number of which depends on the number of processors in the system.
        /// </summary>
        private readonly ThreadWorker[] threadWorkers = new ThreadWorker[Environment.ProcessorCount];

        /// <summary>
        /// The counter of completed threads, allows determining the successful completion of the calculation.
        /// </summary>
        private int completeCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureCalculator"/> class.
        /// Initializes worker threads and subscribes to their events.
        /// </summary>
        public SignatureCalculator()
        {
            for (var i = 0; i < this.threadWorkers.Length; i++)
            {
                var threadWorker = new ThreadWorker();
                threadWorker.SignatureCalculated += this.SignatureCalculator_SignatureCalculated;
                threadWorker.ThreadCompleted += this.SignatureCalculator_ThreadCompleted;

                this.threadWorkers[i] = threadWorker;
            }
        }

        /// <summary>
        /// Start of the multi-thread calculation process.
        /// </summary>
        public void Calculate()
        {
            foreach (var threadWorker in this.threadWorkers)
            {
                new Thread(threadWorker.Run).Start();
            }
        }

        /// <summary>
        /// The signature calculated event handler.
        /// Print the result of the block calculation.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The calculation result parameters. </param>
        private void SignatureCalculator_SignatureCalculated(object sender, ThreadWorkerEventArgs e)
        {
            Console.WriteLine("{0}:\t{1}", e.BlockNumber, e.HashValue);
        }

        /// <summary>
        /// The thread completed event handler.
        /// Check that we have completed all block calculations.
        /// </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void SignatureCalculator_ThreadCompleted(object sender, EventArgs e)
        {
            Interlocked.Increment(ref this.completeCount);
            if (this.completeCount == this.threadWorkers.Length)
            {
                Console.WriteLine("Completed.");
            }
        }
    }
}
