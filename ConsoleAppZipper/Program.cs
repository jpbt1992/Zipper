using ConsoleAppZipper.Output;
using System;

namespace ConsoleAppZipper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 6)
                {
                    Console.WriteLine("Insufficient arguments provided.");
                    return;
                }

                string folderToZip = args[0];
                string zipFileName = args[1];
                string[] excludedExtensions = args[2].Split(',');
                string[] excludedDirectories = args[3].Split(',');
                string[] excludedFiles = args[4].Split(',');
                OutputType outputType = Enum.Parse<OutputType>(args[5], true);
                string additionalParameter = args.Length > 6 ? args[6] : null;

                FolderZipper.Instance.Create(folderToZip, zipFileName, excludedExtensions, excludedDirectories, excludedFiles);

                IOutput output = OutputFactory.Instance.CreateOutput(outputType, additionalParameter);
                output.Save(zipFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
