using System;
using System.IO;

namespace VEEAM_Signature
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //args = new[] { "C:\\Users\\IShemetov\\source\\repos\\VEEAM Signature\\File20GB", "16000" };
            string filePath;
            int blockSize;

            if (args.Length < 2)
            {
                Console.Write("Enter file path: ");
                filePath = Console.ReadLine();

                Console.Write("Enter block size: ");
                while (!int.TryParse(Console.ReadLine(), out blockSize))
                {
                    Console.WriteLine("Oops, incorrect size. Try again.");
                }
            }
            else
            {
                filePath = args[0];
                if (!int.TryParse(args[1], out blockSize))
                {
                    Console.WriteLine("Incorrect size.");
                    return;
                }
            }

            if (File.Exists(filePath))
            {
                FileStreamer.Initialization(filePath, blockSize);

                new SignatureCalculator().Calculate();
            }
            else
            {
                Console.WriteLine("Damn! the file specified could not be found.");
            }
        }
    }
}
