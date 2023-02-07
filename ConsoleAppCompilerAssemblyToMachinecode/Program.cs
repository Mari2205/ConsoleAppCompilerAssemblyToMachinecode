using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCompilerAssemblyToMachinecode
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Paraser paraser = new Paraser();
            paraser.ParseAsmToHack("");

        }
    }
}
