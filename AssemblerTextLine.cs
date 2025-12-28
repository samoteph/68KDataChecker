using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public class AssemblerTextLine
    {
        private string[] tokens = new string[]
        {
            "move",
            "moveq",
            "movea",
            "movem",
            "movep",
            "cmp",
            "cmpi",
            "cmpa",
            "tst",
            "add",
            "addq",
            "addi",
            "adda",
            "addx",
            "sub",
            "subq",
            "subi",
            "suba",
            "subx",
            "mulu",
            "divu",
            "muls",
            "divs",
            "asr",
            "asl",
            "lsr",
            "lsl",
            "rol",
            "ror",
            "roxl",
            "roxr",
            "swap",
            "neg",
            "negx",
            "and",
            "andi",
            "or",
            "ori",
            "eor",
            "eori",
            "not",
            "clr",
            "btst",
            "bset",
            "bclr",
            "bchg",
            "abcd",
            "sbcd",
            "nbcd",
        };

        public AssemblerTextLine(AssemblerTextFile file, int number, string content)
        {
            this.File = file;
            Number = number;
            Content = content;
        }

        public AssemblerTextFile File { get; }

        public string SearchIncludeRelativePath()
        {
            if(string.IsNullOrEmpty(this.Content))
            {
                return string.Empty;
            }

            return this.Content.IgnoreSpace().SearchToken("include").IgnoreSpace().ExtractString(false);
        }
        public AssemblerTextData? SearchData()
        {
            if (string.IsNullOrEmpty(this.Content))
            {
                return null;
            }

            Func<string, AssemblerType> getAssemblerType = content =>
            {
                content = content.SearchToken("dc", "ds");

                if (content == null)
                {
                    return AssemblerType.Unknown;
                }

                if (content.StartsWith("."))
                {
                    return content.Pass(1).ExtractCharacter().GetAssemblerType();
                }
                else
                {
                    return AssemblerType.Word;
                }

                return AssemblerType.Unknown;
            };

            // data ou bss avec label 

            string name = null;

            AssemblerType type = AssemblerType.Unknown;

            var label = this.Content.ExtractString(false, ':');

            if(!String.IsNullOrEmpty(label))
            {
                if(label.EndsWith(":"))
                {
                    name = label.Substring(0, label.Length - 1);
                }
                else
                {
                    name = label;
                }

                var content = this.Content.Pass(label.Length).IgnoreSpace();

                if(content != null)
                {
                    type = getAssemblerType(content);
                }
            }
            else
            {
                //recherche de dc ou ds puis du label sur les lignes d'avant
                type = getAssemblerType(this.Content.IgnoreSpace());
            
                if(type != AssemblerType.Unknown)
                {
                    // rechercher le label dans les lignes d'avant
                    
                    for(int i=Number-1-1;i>=0;i--)
                    {
                        var line = File.Lines[i];

                        var labelFromOtherLine = line.Content.ExtractString(false, ':');
                        
                        if(!string.IsNullOrEmpty(labelFromOtherLine))
                        {
                            if (labelFromOtherLine.EndsWith(":"))
                            {
                                name = labelFromOtherLine.Substring(0, label.Length - 1);
                            }
                            else
                            {
                                name = labelFromOtherLine;
                            }

                            break;
                        }

                        if (line.Content.IgnoreSpace() != null)
                        {
                            break;
                        }
                    }


                }
            }

            if (name != null && name != ";" && type != AssemblerType.Unknown)
            {
                return new AssemblerTextData(name, type);
            }

            return null;

        }

        internal AssemblerTextInstruction SearchInstruction()
        {
            var content = this.Content.IgnoreSpace();

            var instructionName = content.GetToken(tokens);
            AssemblerType assemblerType = AssemblerType.Unknown;

            if(instructionName != null)
            {
                instructionName = instructionName.ToLower();
                
                content = content.Pass(instructionName.Length);

                if(content.StartsWith("."))
                {
                    assemblerType = content.Pass(1).ExtractCharacter().GetAssemblerType();
                    content = content.Pass(2);
                }
                else
                {
                    assemblerType = AssemblerType.Unknown;
                }

                var parameter = content.IgnoreSpace().ExtractString(false, ',');

                string parameter1 = null;
                string parameter2 = null;

                if(parameter != null)
                {
                    content = content.IgnoreSpace().Pass(parameter.Length);

                    if (content != null && content.StartsWith(","))
                    {
                        parameter1 = parameter;
                        parameter2 = content.Pass(1).ExtractString(false);
                    }
                    else
                    {
                        parameter1 = parameter;
                    }
                }

                return new AssemblerTextInstruction(
                    this,
                    instructionName,
                    assemblerType,
                    parameter1,
                    parameter2
                    );

            }

            return null;
        }

        public int Number { get; }
        public string Content { get; }
    }
}
