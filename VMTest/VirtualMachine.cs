using System;
using System.Collections.Generic;
using System.Text;

class VirtualMachine
{
    private Dictionary<string, float> valMap = new Dictionary<string, float>();

    private List<VMNode> nodeList;
    private Stack<float> vmStack;
    private Dictionary<string, float> variableMap;

    public VirtualMachine(List<VMNode> clint)
    {
        nodeList = clint;
        vmStack = new Stack<float>();
        variableMap = new Dictionary<string, float>();
    }

    public void Run()
    {
        for (int i = 0; i < nodeList.Count; ++i)
        {
            var node = nodeList[i];
            switch (node.Type)
            {
                case VMNodeType.VALUE:
                    {
                        vmStack.Push((float)node.Value);
                    }
                    break;
                case VMNodeType.NAME:
                    {
                        vmStack.Push(variableMap.GetValueOrDefault((string)node.Value));
                    }
                    break;
                case VMNodeType.FUNCTION:
                    {
                        float arg1, arg2;
                        arg2 = vmStack.Pop();
                        arg1 = vmStack.Pop();
                        switch ((string)node.Value)
                        {
                            case "plus":
                                vmStack.Push(arg1 + arg2);
                                break;
                            case "minus":
                                vmStack.Push(arg1 - arg2);
                                break;
                            case "times":
                                vmStack.Push(arg1 * arg2);
                                break;
                            case "div":
                                vmStack.Push(arg1 / arg2);
                                break;
                        }
                    }
                    break;
                case VMNodeType.VARIABLE:
                    {
                        variableMap[(string)node.Value] = vmStack.Pop();
                    }
                    break;
                case VMNodeType.IFGOTO:
                    {
                        float arg = vmStack.Pop();
                        if (arg == 0)
                        {
                            i = (int)node.Value - 1;
                        }
                    }
                    break;
                case VMNodeType.GOTO:
                    {
                        i = (int)node.Value - 1;
                    }
                    break;
            }
        }
        int cnt = 0;
        Console.WriteLine("-> VM");
        while (vmStack.Count > 0)
        {
            Console.WriteLine("\tVM-Stack <" + cnt++.ToString() + "> : " + vmStack.Pop());
        }
        Console.WriteLine("\tEND OF PROGRAM.");
        Console.WriteLine();
        // Console.ReadKey();
    }
}