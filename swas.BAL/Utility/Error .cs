namespace swas.BAL.Utility
{
    public class Error
    {
        //public static int ExceptionHandle(string message)
        //{
        //    string filePath = @"\Error.txt";
        //    string docPath = "wwwroot/Error";
        //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //    var _count = File.ReadAllText(docPath + "/Error.txt");
        //    // Write the string array to a new file named "WriteLines.txt".
        //    using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, "Error.txt")))
        //    {

        //        outputFile.WriteLine(_count);
        //        outputFile.WriteLine("-----------------------------------------------------------------------------");
        //        outputFile.WriteLine("Date : " + DateTime.Now.ToString());

        //        outputFile.WriteLine(message);
        //    }
        //    return 0;

        //}

        public static int ExceptionHandle(string message)
        {
            string docPath = "wwwroot/Error";
            string filePath = Path.Combine(docPath, "Error.txt");

            // Read all lines from the file
            List<string> lines = File.ReadAllLines(filePath).ToList();

            // Add the new message
            lines.Add("-----------------------------------------------------------------------------");
            lines.Add("Date : " + DateTime.Now.ToString());
            lines.Add(message);

            // Remove older lines if the total number of lines exceeds 200
            if (lines.Count > 900)
            {
                int startIndex = lines.Count - 900; // Keep the most recent 200 lines
                lines = lines.Skip(startIndex).ToList();
            }

            // Write the modified lines back to the file
            File.WriteAllLines(filePath, lines);

            return 0;
        }


    }
}
