using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using SimpleScript.Analyzing;
using SimpleScript.RunTime;

namespace SimpleScript.Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length<0 )
            {
                Console.WriteLine("Usage: SimpleScript.exe <Sourcefile.ss>");
                return;
            }

            FileStream file = null;
            Scanner scn = null;
            Parser parser = null;
            StatementList program = null;

            try
            {
                //file = new FileStream(args[0], FileMode.Open);
                file = new FileStream("Script3.ss", FileMode.Open);
                scn = new Scanner(file);
                parser = new Parser(scn);
                parser.Parse();
                program = parser.program;

                if (program != null)
                {
                    program.Execute();
                    Console.WriteLine();
                }
            }
            finally
            {
                file.Close();
            }
            
            
        }
    }
}
