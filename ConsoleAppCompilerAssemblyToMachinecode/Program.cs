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
            paraser.testing();
            //convertNumToByte(1);
        }

        public static void convertNumToByte(int num)
        {
            var f = Convert.ToInt16("2",16);
            var bin = Convert.ToString(2,16);
            var bind = Convert.ToString(4, 2);

            var cout = bind.Length;
            var bitstring = "0000000000000000";
            var sb = new StringBuilder(bitstring);

            var fg = bind.Reverse().ToArray();
            var bitfull = "";//fg.ToString();
            var incot = 16 - cout;
            for (int i = 0; i < incot; i++)
            {
                bitfull = bitfull + "0";
            }
            var tere = bitfull + bind;
            //foreach (var i in bind)
            //{
            //    sb.Replace();
            //}
            //for (int i = 15; i < 0; i--)
            //{
            //    sb 
            //}
            //return f;
        }
    }
}
