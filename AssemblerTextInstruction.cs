using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public class AssemblerTextInstruction
    {
        public AssemblerTextInstruction(AssemblerTextLine line, string name, AssemblerType assemblerType, string parameter1 = null, string parameter2 = null) 
        {
            this.Line = line;
            this.Name = name;
            this.AssemblerType = assemblerType;

            if (parameter1 != null)
            {
                this.ParameterCounter += 1;
                this.Parameter1 = parameter1;

                if (parameter2 != null)
                {
                    this.ParameterCounter += 1;
                    this.Parameter2 = parameter2;
                }
            }
        }

        public AssemblerTextLine Line { get;}
        public AssemblerType AssemblerType 
        { 
            get; 
        }

        public string Name 
        { 
            get ; 
        }

        public int ParameterCounter
        {
            get;
        }

        public string Parameter1
        {
            get;
        }

        public string Parameter2
        {
            get;
        }

        public override string ToString()
        {
            string content = $"{Name}.{AssemblerType}";
        
            if(Parameter1 != null)
            {
                content += " " + Parameter1; 
            }

            if (Parameter2 != null)
            {
                {
                    content += "," + Parameter2;
                }
            }

            return content;
        }
    }
}
