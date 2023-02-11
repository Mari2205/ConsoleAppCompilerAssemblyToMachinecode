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
            var asmfile = GetFile(); 

            List<string> bitList = new List<string>();

            var asmFileClean = Utilities.CleanUpFile(asmfile);

            var labelsInFile = FindLabels(asmFileClean);
            var labelsReplaced = HandleLabels(labelsInFile, asmFileClean);

            var sysmblosInFile = FindCustomSysmblos(labelsReplaced);
            var sysblosReplaced = HandleSysmblos(labelsReplaced, sysmblosInFile);

            foreach (var line in sysblosReplaced)
            {
                if (line.StartsWith("@"))
                {
                    bitList.Add(handelAInstruktion(line));

                }
                else
                {
                    bitList.Add(handelCInstruktion(line));
                }
            }
            WriteFile(bitList);
        }

        #region File handeling
        private string[] GetFile()
        {
            var basePath = @"C:\Users\uncha\Desktop\nand2tetris\projects\06\";
            var pathToAdd = basePath + @"add\Add.asm";
            var pathToMaxL = basePath + @"max\MaxL.asm";
            var pathToPongL = basePath + @"add\PongL.asm";
            var pathToRectL = basePath + @"add\RectL.asm";

            var pathToMax = basePath + @"max\Max.asm"; 
            var pathToPong = basePath + @"pong\Pong.asm";
            var pathToRect = basePath + @"rect\Rect.asm";

            var output = file.ReadAsmFile(pathToRect);

            return output;
        }

        private void WriteFile(List<string> bitList)
        {
            file.WriteHackFile(bitList, @"C:\Users\uncha\Desktop\myRect.hack");
        }
        #endregion

        #region Handel labels, sysmblos
        public Dictionary<string, string> FindLabels(List<string> asmFile)
        {
            Dictionary<string, string> Labels = new Dictionary<string, string>();
            int index = 0;
            foreach (var line in asmFile)
            {
                if (line.Contains("("))
                {
                    var findLabelText = line.Replace("(", String.Empty).Replace(")", String.Empty).Replace(" ", String.Empty);
                    Labels.Add(findLabelText, index.ToString());
                    index--;
                }
                index++;
            }
            return Labels;
        }

        public List<String> HandleLabels(Dictionary<string, string> labels, List<string> asmFile)
        {
            List<string> asmWhitoutLabels = new List<string>();
            foreach (var item in asmFile)
            {
                if (item.Contains("@"))
                {
                    var rmAtSign = item.Replace("@", String.Empty);
                    foreach (var lableItem in labels)
                    {
                        if (lableItem.Key == rmAtSign)
                        {
                            asmWhitoutLabels.Add("@" + lableItem.Value);

                        }
                    }

                    Dictionary<string, string> predMemLoc = SetStandartPortTable();
                    foreach (var symboles in predMemLoc)
                    {
                        if (symboles.Key == rmAtSign)
                        {
                            asmWhitoutLabels.Add("@" + symboles.Value);
                        }
                    }

                    if (!predMemLoc.ContainsKey(rmAtSign) && !labels.ContainsKey(rmAtSign))
                    {
                        asmWhitoutLabels.Add(item);
                    }
                }
                else if (!item.Contains("("))
                {
                    asmWhitoutLabels.Add(item);
                }
            }
            return asmWhitoutLabels;
        }

        public Dictionary<string, string> FindCustomSysmblos(List<string> asmFile)
        {
            Dictionary<string, string> sysboles = new Dictionary<string, string>();
            var index = 16;
            foreach (var item in asmFile)
            {
                if (item.Contains("@"))
                {
                    var rmAtSign = item.Replace("@", String.Empty);
                    if (!sysboles.ContainsKey(rmAtSign))
                    {
                        if (!item.Replace("@", "").All(c => char.IsDigit(c)))
                        {
                            var ri = item;
                            sysboles.Add(rmAtSign, index.ToString());
                            index++;
                        }
                    }

                }
            }
            return sysboles;
        }

        public List<string> HandleSysmblos(List<string> asmFile, Dictionary<string, string> sysbols)
        {
            List<string> output = new List<string>();
            foreach (var item in asmFile)
            {
                if (item.Contains("@"))
                {
                    var rmAtSign = item.Replace("@", String.Empty);
                    if (sysbols.ContainsKey(rmAtSign))
                    {
                        foreach (var sysitem in sysbols)
                        {
                            if (sysitem.Key == rmAtSign)
                            {
                                output.Add("@" + sysitem.Value);
                            }
                        }
                    }
                    else
                    {
                        output.Add(item);
                    }
                }
                else
                {
                    output.Add(item);
                }
            }
            return output;
        }
        #endregion

        #region Dictornary loop, splits
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
            }
            return oneLine;
            
        }

        public Dictionary<string, string> SplitUpByKol(string line)
        {
            Dictionary<string, string> DCJDic = new Dictionary<string, string>();

            var Fistsplit = line.Split(';');
            var dest = "";
            var comp = Fistsplit[0];
            var jump = Fistsplit[1];

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
        #endregion

        #region A and C instruktion
        public string handelAInstruktion(string line)
        {
            var split = line.Split('@');
            var num = Convert.ToInt32(split[1]);

            var bits = Utilities.ConvertNumToByte(num);
            return bits;
        }

        private string handelCInstruktion(string line)
        {
            if (line.Contains("="))
            {
                var DCJ = SplitUpByEQ(line);
                var dest = DCJ["dest"];
                var comp = DCJ["comp"];
                var jump = DCJ["jump"];

                return LoopThowDictornary(dest, comp, jump);

            }
            else
            {
                var DCJ = SplitUpByKol(line);
                var dest = DCJ["dest"];
                var comp = DCJ["comp"];
                var jump = DCJ["jump"];

                return LoopThowDictornary(dest, comp, jump);

            }
        }
        #endregion

        #region Dictionarys
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

        public Dictionary<string, string> SetStandartPortTable()
        {
            Dictionary<string, string> standartPortTable = new Dictionary<string,string>();

            standartPortTable.Add("R0", "0");
            standartPortTable.Add("R1", "1");
            standartPortTable.Add("R2", "2");
            standartPortTable.Add("R3", "3");
            standartPortTable.Add("R4", "4");
            standartPortTable.Add("R5", "5");
            standartPortTable.Add("R6", "6");
            standartPortTable.Add("R7", "7");
            standartPortTable.Add("R8", "8");
            standartPortTable.Add("R9", "9");
            standartPortTable.Add("R10", "10");
            standartPortTable.Add("R11", "11"); 
            standartPortTable.Add("R12", "12");
            standartPortTable.Add("R13", "13");
            standartPortTable.Add("R14", "14");
            standartPortTable.Add("R15", "15");
            standartPortTable.Add("SP","0");
            standartPortTable.Add("LCL", "1");
            standartPortTable.Add("ARG", "2");
            standartPortTable.Add("THIS", "3");
            standartPortTable.Add("THAT", "4");
            standartPortTable.Add("SCREEN", "16384");
            standartPortTable.Add("KBD", "24576");

            return standartPortTable;
        }
        #endregion
    }
}
