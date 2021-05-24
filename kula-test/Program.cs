using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    public static string[] formatBlank = new string[]
    {
        "          ",
        "         ",
        "        ",
        "       ",
        "      ",
        "     ",
        "    ",
        "   ",
        "  ",
        " ",
    };
    private static void HelloKula()
    {
        Console.WriteLine("Kula test - alpha2 [2021/5/24] (on .net Framework at least 4.6)");
        Console.WriteLine("developed by @HanaYabuki in github.com");
    }
    private static void DebugRunCode(string code)
    {
        VMLexer vmLexer = new VMLexer(code);
        vmLexer.Scan();
        vmLexer.Show();
        VMParser vmParser = new VMParser(vmLexer.TokenStream);
        vmParser.Parse();
        vmParser.Show();
        VirtualMachine vm = new VirtualMachine(vmParser.NodeStream);
        vm.Run();
    }
    private static void Main(string[] args)
    {
        /**
        // Console.WriteLine(args);
        VMLexer vmLexer = new VMLexer(
            // "15;"
            // "var = 10; var;"
            // "var = minus(plus(20, 5), div(10, 2)); times(var, 0.137);"
            // "if(-1){1;2;3;}4;"
            "cnt = 15; while(cnt){cnt = minus(cnt, 1); cnt; }"
            );
        vmLexer.Scan();
        vmLexer.Show();
        VMParser vmParser = new VMParser(vmLexer.TokenStream);
        vmParser.Parse();
        vmParser.Show();
        VirtualMachine vm = new VirtualMachine(vmParser.NodeStream);
        */
        if (args.Length == 0)
        {
            HelloKula();
            string code;
            while (true)
            {
                Console.Write(">> ");
                code = Console.ReadLine();
                if (code == "exit")
                {
                    break;
                }
                DebugRunCode(code);
            }
        }
        else
        {
            string code;
            try
            {
                code = File.ReadAllText(args[0]);
            }
            catch
            {
                Console.WriteLine("!!! ERROR : Can not open the file. !!!");
                return;
            }
            DebugRunCode(code);
        }
    }
}
