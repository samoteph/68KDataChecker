using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _68KDataChecker
{
    public class AssemblerTextData
    {
        public AssemblerTextData(string name, AssemblerType assemblerType)
        {
            this.Name = name;
            this.AssemblerType = assemblerType;

            this.SmallType = char.ToLower(this.AssemblerType.ToString()[0]);
            
        }

        public string Name
        {
            get;
        }

        public AssemblerType AssemblerType
        {
            get;
        }

        public char SmallType
        {
            get;
        }
    }

    public enum AssemblerType
    {
        Unknown,
        Byte,
        Word,
        Long
    }
}
