namespace _68KDataChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("68K data checker - Samuel Blanchard 2025");
            Console.ForegroundColor= ConsoleColor.White;

            string fullFilenameRoot = args[0];

            Console.WriteLine($"loading project from '{fullFilenameRoot}'");
            var project = AssemblerTextProject.Load(fullFilenameRoot);

            project.Initialize();

            Console.WriteLine($"Collecting data...");
            var dataCollection = project.CollectDataAndBss().ToArray();
            Console.WriteLine($"Collecting instructions...");
            var instructionCollection = project.CollectInstructions().ToArray();

            int errorCounter = 0;

            Console.WriteLine($"Analyzing {instructionCollection.Length} instructions in {project.Files.Count} files");
            
            foreach ( var data in dataCollection)
            {
                foreach( var instruction in instructionCollection)
                {
                    if (data.AssemblerType != instruction.AssemblerType)
                    {
                        if (instruction.Parameter1 == data.Name || instruction.Parameter2 == data.Name)
                        {
                            errorCounter++;

                            // Traitement de l'incoherence
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"{data.Name}.{data.AssemblerType}");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"!= File {instruction.Line.File.Path}; Line {instruction.Line.Number}, instruction: {instruction.ToString()}");
                        }
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            
            if(errorCounter > 0)
            {
                Console.WriteLine($"Potential bug found: {errorCounter}");
            }
            else
            {
                Console.WriteLine($"Your code is clean!");
            }
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.WriteLine("End Check instructions");
        }
    }
}
