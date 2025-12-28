using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public class AssemblerTextFile
    {
        public string Path { get; }
        public AssemblerTextLine[] Lines { get; private set; }

        private AssemblerTextFile(string path)
        {
            Path = path;
        }

        public static AssemblerTextFile Load(string path)
        {
            int count = 0;

            var file = new AssemblerTextFile(path);

            var lines = new List<AssemblerTextLine>();

            foreach(var line in File.ReadLines(path))
            {
                lines.Add(new AssemblerTextLine(file, ++count, line));
            }

            file.Lines = lines.ToArray();

            return file;
        }

    }
}
