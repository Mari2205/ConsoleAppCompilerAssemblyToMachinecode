using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleAppCompilerAssemblyToMachinecode
{
    public class FileHandling
    {
        public string[] ReadAsmFile(string asmFilePath)
        {
            string[] contents = System.IO.File.ReadAllLines(asmFilePath);
            return contents;
        }


        /// <summary>
        /// this method craetes a new .hack file form a byte[] 
        /// </summary>
        /// <param name="hackFileContents"></param>
        /// <param name="pathToPlaceFile">the location to put the new file</param>
        public void WriteHackFile(List<string> hackFileContents, string pathToPlaceFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(pathToPlaceFile))
            {
                foreach (var line in hackFileContents)
                {
                    streamWriter.WriteLine(line);
                }

            }
        }

        ///// <summary>
        ///// this method craetes a new .hack file form a byte[] 
        ///// </summary>
        ///// <param name="hackFileContents"></param>
        ///// <param name="pathToPlaceFile">the location to put the new file</param>
        //public void WriteHackFile(string[] hackFileContents, string pathToPlaceFile)
        //{
        //    using (StreamWriter streamWriter = new StreamWriter(pathToPlaceFile))
        //    {
        //        foreach (var line in hackFileContents)
        //        {
        //            streamWriter.WriteLine(line);
        //        }

        //    }
        //}
    }
}
