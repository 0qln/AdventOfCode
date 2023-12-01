namespace FileTreeGenerator
{
    class Program()
    {
        static int Main(string[] args)
        {
            Console.WriteLine("For what year would you like the file tree to be generated?");
            string year = Console.ReadLine() ?? "";

            // Create year dir
            string yearDir = Path.Combine(GetAOC() ?? throw new ArgumentException(), year);
            Directory.CreateDirectory(yearDir);

            // Create the day dirs
            CreatePathsFor(1, 25, GetDay, yearDir, partDir =>

            // Create the parts of the day
            CreatePathsFor(1, 2, GetPart, partDir));

            return 0;
        }

        /// <summary>
        /// Create a range of directories.
        /// </summary>
        /// <param name="start">Inclusive.</param>
        /// <param name="end">Inclusive.</param>
        /// <param name="formatter">Formatter for the name of the newly created directories.</param>
        /// <param name="location">The location that the new directories will be created in.</param>
        /// <param name="iterationCallback">An optional callback that will be invoced after each creted directory.</param>
        static void CreatePathsFor(int start, int end, Func<int, string> formatter, string location, Action<string>? iterationCallback = null)
        {
            for (int i = start; i <= end; i++)
            {
                string dir = Path.Combine(location, formatter(i));
                Directory.CreateDirectory(dir);
                iterationCallback?.Invoke(dir);
            }
        }

        static string GetPart(int part) => "Part_" + part.ToString();

        static string GetDay(int day) => "Day_" + day.ToString();

        static string? GetAOC()
        {
            var aocDir = Environment.CurrentDirectory;
            for (int i = 0; i < 6; i++) aocDir = aocDir?.GetParent();
            return aocDir;
        }
    }
}
