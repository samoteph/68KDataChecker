using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public class AssemblerTextProject
    {
        private Dictionary<string, AssemblerTextFile> files = new Dictionary<string, AssemblerTextFile>(StringComparer.OrdinalIgnoreCase);
        public ReadOnlyDictionary<string, AssemblerTextFile> Files
        {
            get
            {
                return files.AsReadOnly();
            }
        }

        public string ProjectPath { get;}

        public AssemblerTextFile RootFile { get; private set; }

        private AssemblerTextProject(string fullFilenameRoot)
        {
            this.ProjectPath = Path.GetDirectoryName(fullFilenameRoot);

            this.RootFile = AssemblerTextFile.Load(fullFilenameRoot);
        }

        public void Initialize()
        {
            IncludeFiles(this.RootFile);
        }

        private string GetAbsolutePath(string relativePath)
        {
            return Path.Combine(ProjectPath,relativePath);
        }

        private void IncludeFile(AssemblerTextFile file)
        {
            files.Add(file.Path, file);
        }

        /// <summary>
        /// Charge un fichier et l'inclut dans le projet.
        /// </summary>
        /// <param name="relativeIncludePath"></param>
        /// <returns></returns>

        private AssemblerTextFile IncludeFile(string relativeIncludePath)
        {
            if (relativeIncludePath == null) return null;

            if(files.ContainsKey(relativeIncludePath))
            {
                return null;
            }

            string absoluteIncludePath = GetAbsolutePath(relativeIncludePath);

            var fileToInclude = AssemblerTextFile.Load(absoluteIncludePath);

            files.Add(relativeIncludePath, fileToInclude);

            return fileToInclude;
        }

        private void IncludeFiles(AssemblerTextFile file)
        {
            if(file == null)
            {
                return;
            }

            foreach (var line in file.Lines)
            {
                var relativeIncludePath = line.SearchIncludeRelativePath();

                if (!string.IsNullOrEmpty(relativeIncludePath))
                {
                    IncludeFiles(IncludeFile(relativeIncludePath));
                }
            }
        }

        public static AssemblerTextProject Load(string fullFilenameRoot)
        {            
            return new AssemblerTextProject(fullFilenameRoot);
        }

        public IEnumerable<AssemblerTextData> CollectDataAndBss()
        {
            foreach (var file in files.Values)
            {
                foreach(var line in file.Lines)
                {
                    var data = line.SearchData();

                    if(data != null)
                    {
                        yield return data;
                    }
                }
            }
        }

        public IEnumerable<AssemblerTextInstruction> CollectInstructions()
        {
            foreach (var file in files.Values)
            {
                foreach (var line in file.Lines)
                {
                    var instruction = line.SearchInstruction();

                    if (instruction != null)
                    {
                        yield return instruction;
                    }
                }
            }
        }
    }
}
