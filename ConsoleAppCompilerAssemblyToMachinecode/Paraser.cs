using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConsoleAppCompilerAssemblyToMachinecode
{
    public class Paraser
    {
        FileHandling file = new FileHandling();

        public void ParseAsmToHack(string asmPath)
        {
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\add\Add.asm");
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\max\MaxL.asm");
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\pong\PongL.asm");
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\rect\RectL.asm");

            var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\max\Max.asm");
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\pong\Pong.asm");
            //var asmfile = file.ReadAsmFile(@"C:\Users\uncha\Desktop\nand2tetris\projects\06\rect\Rect.asm");

            List<string> bitList = new List<string>();
            string[] asmWithoutComments = ReCodeComments(asmfile);


            foreach (var line in asmWithoutComments)
            {
                if (line.StartsWith("@"))
                {
                    bitList.Add(MakeAInstruktion(line));

                }
                else if (line.Contains("="))
                {
                    var DCJ = SplitUpByEQ(line);
                    var dest = DCJ["dest"];
                    var comp = DCJ["comp"];
                    var jump = DCJ["jump"];

                    bitList.Add(LoopThowDictornary(dest, comp, jump));

                }
                else
                {
                    var DCJ = SplitUpByKol(line);
                    var dest = DCJ["dest"];
                    var comp = DCJ["comp"];
                    var jump = DCJ["jump"];

                    bitList.Add(LoopThowDictornary(dest, comp, jump));

                }
            }
            //WriteToConsole(bitList);
            FileHandling fileHandler = new FileHandling();
            fileHandler.WriteHackFile(bitList, @"C:\Users\uncha\Desktop\myRectL.hack");
        }

        public string MakeAInstruktion(string line)
        {
            var split = line.Split('@');
            var num = Convert.ToInt32(split[1]);

            var f = convertNumToByte(num);
            return f;
        }

        public void WriteToConsole(List<string> hackfile)
        {
            foreach (var item in hackfile)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }

        public string LoopThowDictornary(string dest, string comp, string jump)
        {
            var oneLine = "111";
            var compdir = SetCompTable();
            var destdir = SetDestTable();
            var jumpdir = SetJumpTable();


            foreach (var item in compdir)
            {
                if (item.Key == comp)
                {
                    oneLine = oneLine + item.Value;
                }
            }

            foreach (var item in destdir)
            {
                if (item.Key == dest)
                {
                    oneLine = oneLine + item.Value;
                }
            }

            foreach (var item in jumpdir)
            {
                if (item.Key == jump)
                {
                    oneLine = oneLine + item.Value;
                }
                //else
                //{
                //    oneLine = oneLine + jump;
                //}
            }

            //bitList.Add(oneLine);
            return oneLine;
            
        }

        public Dictionary<string, string> SplitUpByKol(string line)
        {
            Dictionary<string, string> DCJDic = new Dictionary<string, string>();

            var Fistsplit = line.Split(';');
            var dest = "";
            var comp = Fistsplit[0];
            var jump = Fistsplit[1];


            //if (Fistsplit[1].Contains(';'))
            //{
            //    var Secondsplit = Fistsplit[1].Split(';');
            //    comp = Secondsplit[0];
            //    jump = Secondsplit[1];
            //}
            //else
            //{
            //    comp = Fistsplit[1];
            //    jump = "";
            //}

            DCJDic.Add("dest", dest);
            DCJDic.Add("comp", comp);
            DCJDic.Add("jump", jump);
            return DCJDic;
        }

        public Dictionary<string, string> SplitUpByEQ(string line)
        {
            Dictionary<string, string> DCJDic = new Dictionary<string, string>();

            var Fistsplit = line.Split('=');
            var dest = Fistsplit[0];
            var comp = string.Empty;
            var jump = "000";

            //string tetererr = Fistsplit[1];
            //bool tttteee = tetererr.Contains(";");
            //if (Fistsplit[1].Contains(";"))
            if (Fistsplit[1].Contains(';'))
            {
                var Secondsplit = Fistsplit[1].Split(';');
                comp = Secondsplit[0];
                jump = Secondsplit[1];
            }
            else
            {
                comp = Fistsplit[1];
                jump = "";
            }

            DCJDic.Add("dest", dest);
            DCJDic.Add("comp", comp);
            DCJDic.Add("jump", jump);
            return DCJDic;
        }

        public string convertNumToByte(int num)
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
        public string[] ReCodeComments(string[] asmFile)
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
        
        public Dictionary<string, string> SetCompTable()
        {
            Dictionary<string, string> compTable = new Dictionary<string, 
                string>();
            compTable.Add("0","0101010");
            compTable.Add("1", "0111111");
            compTable.Add("-1", "0111010");
            compTable.Add("D", "0001100");
            compTable.Add("A", "0110000");
            compTable.Add("!D", "0001101");
            compTable.Add("!A", "0110001");
            compTable.Add("-D", "0001111");
            compTable.Add("-A", "0110011");
            compTable.Add("D+1", "0011111");
            compTable.Add("A+1", "0110111");
            compTable.Add("D-1", "0001110");//0011110
            compTable.Add("A-1", "0110010");
            compTable.Add("D+A", "0000010");//
            compTable.Add("D-A", "0010011");
            compTable.Add("A-D", "0000111");
            compTable.Add("D&A", "0000000");
            compTable.Add("D|A", "0010101");

            compTable.Add("M", "1110000");
            compTable.Add("!M", "1110001");
            compTable.Add("-M", "1110011");
            compTable.Add("M+1", "1110111");
            compTable.Add("M-1", "1110010");//1110011
            compTable.Add("D+M", "1000010");
            compTable.Add("D-M", "1010011");
            compTable.Add("M-D", "1000111");
            compTable.Add("D&M", "1000000");
            compTable.Add("D|M", "1010101");

            return compTable;
        }

        public Dictionary<string, string> SetDestTable()
        {
            Dictionary<string, string> destTable = new Dictionary<string,
                string>();
            destTable.Add("", "000");
            destTable.Add("M", "001");
            destTable.Add("D", "010");
            destTable.Add("MD", "011");
            destTable.Add("A", "100");
            destTable.Add("AM", "101");
            destTable.Add("AD", "110");
            destTable.Add("AMD", "111");

            return destTable;
        }

        public Dictionary<string, string> SetJumpTable()
        {
            Dictionary<string, string> jumpTable = new Dictionary<string,
                string>();
            jumpTable.Add("", "000");
            jumpTable.Add("JGT", "001");
            jumpTable.Add("JEQ", "010");
            jumpTable.Add("JGE", "011");
            jumpTable.Add("JLT", "100");
            jumpTable.Add("JNE", "101");
            jumpTable.Add("JLE", "110");
            jumpTable.Add("JMP", "111");


            return jumpTable;
        }
    }
}
