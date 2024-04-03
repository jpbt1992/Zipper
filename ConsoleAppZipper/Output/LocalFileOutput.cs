using System;
using System.IO;

namespace ConsoleAppZipper.Output
{
    public class LocalFileOutput : IOutput
    {
        private readonly string outputPath;

        public LocalFileOutput(string outputPath)
        {
            this.outputPath = outputPath;
        }

        public void Save(string zipFilePath)
        {
            string destinationPath = outputPath ?? Directory.GetCurrentDirectory();
            string outputFilePath = Path.Combine(destinationPath, Path.GetFileName(zipFilePath));

            File.Move(zipFilePath, outputFilePath, true);

            Console.WriteLine($"ZIP file saved to: {outputFilePath}");
        }
    }
}