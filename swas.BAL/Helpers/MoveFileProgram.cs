using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace swas.BAL.Helpers
{
    public class MoveFileProgram
    {
        public static void MoveFile()
        {
            try
            {
                // Define paths
                string destinationFolder = @"D:\Git_Work\swas.UI\wwwroot\Uploads";
                string sourceFolder = @"D:\Git_Work\swas.UI\wwwroot\temp";
                string filePath = @"C:\Users\lanmanager\Desktop\MoveFile.txt";

                // Check if the text file exists
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("MoveFile.txt not found.");
                    return;
                }

                // Read file names from MoveFile.txt
                string[] fileNames = File.ReadAllLines(filePath);

                foreach (string fileName in fileNames)
                {
                    string trimmedFileName = fileName.Trim();
                    if (string.IsNullOrEmpty(trimmedFileName)) continue;

                    string sourceFile = Path.Combine(sourceFolder, trimmedFileName);
                    string destinationFile = Path.Combine(destinationFolder, trimmedFileName);

                    if (File.Exists(sourceFile))
                    {
                        // Move the file
                        File.Move(sourceFile, destinationFile);
                        Console.WriteLine($"Moved: {trimmedFileName}");
                    }
                    else
                    {
                        Console.WriteLine($"File not found: {trimmedFileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
