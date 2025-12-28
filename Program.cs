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

            var project = AssemblerTextProject.Load(fullFilenameRoot);

            project.Initialize();

            Console.WriteLine("Start data construction");
            var dataCollection = project.CollectDataAndBss().ToList();
            var instructionCollection = project.CollectInstructions();
            Console.WriteLine("Stop data construction");

            int errorCounter = 0;

            Console.WriteLine("Start Check instructions");
            
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

            if(errorCounter > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.WriteLine($"Potential bug found: {errorCounter}");
            Console.ForegroundColor= ConsoleColor.White;
            
            Console.WriteLine("End Check instructions");
        }
    }
}
