using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppCompilerAssemblyToMachinecode
{
    public class Utilities
    {
        private static string[] RmCodeComments(string[] asmFile)
        {

            for (int i = 0; i < asmFile.Length; i++)
            {
                if (asmFile[i].Contains("//"))
                {
                    if (asmFile[i].StartsWith("//"))
                    {
                        asmFile[i] = "";
                    }
                    else
                    {
                        var indexOfCumment = asmFile[i].IndexOf("//");
                        var f = asmFile[i].Remove(indexOfCumment);
                        asmFile[i] = f;
                    }

                }
            }

            var content = asmFile.Where(s => s != "").ToArray();
            return content;
        }

        private static List<string> RmWhiteSpaces(string[] strArr)
        {
            List<string> res = new List<string>();
            foreach (var item in strArr)
            {
                res.Add(item.Replace(" ", String.Empty));
            }

            return res;
        }

        public static List<string> CleanUpFile(string[] asmFile)
        {
            var rmCodeCommit = RmCodeComments(asmFile);
            var rmWhiteSpace = RmWhiteSpaces(rmCodeCommit);
            
            return rmWhiteSpace;
        }

        public static void WriteToConsole(List<string> hackfile)
        {
            foreach (var item in hackfile)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        public static string ConvertNumToByte(int num)
        {
            var f = Convert.ToInt16("2", 16);
            var bin = Convert.ToString(2, 16);
            var bind = Convert.ToString(num, 2);

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
            return tere;
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
