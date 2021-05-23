using System;
using System.Collections.Generic;
using System.Text;


class VMParser
{
    private static HashSet<string> functionSet = new HashSet<string>
    {
        "plus", "minus", "times", "div",
    };

    private List<VMLexToken> tokenStream;
    private List<VMNode> nodeStream;

    private Stack<string> funcNameStack;
    private Stack<string> valNameStack;
    private int pos;

    public List<VMNode> NodeStream { get => nodeStream ?? Parse(); }

    public VMParser(List<VMLexToken> tokenStream)
    {
        this.tokenStream = tokenStream;
    }

    public List<VMNode> Parse()
    {
        pos = 0; int _pos = -1;
        this.nodeStream = new List<VMNode>();
        this.funcNameStack = new Stack<string>();
        this.valNameStack = new Stack<string>();
        while (pos < tokenStream.Count && _pos != pos)
        {
            _pos = pos;
            try
            {
                ParseStatement();
            }
            catch
            {
                _pos = -1;
            }
        }
        if (pos != tokenStream.Count)
        {
            nodeStream.Clear();
        }
        return nodeStream;
    }

    public void Show()
    {
        Console.WriteLine("-> Parser");
        if (nodeStream.Count == 0)
        {
            Console.WriteLine("\t!!! ERROR : Can not Parse the code. !!!");
            return;
        }
        foreach (VMNode node in nodeStream)
        {
            if (node != null)
            {
                Console.WriteLine("\t[ " + node.Type + " | " + (node.Value ?? "-") + " ]");
            }
        }
        Console.WriteLine();
    }

    private bool ParseStatement()
    {
        int _pos = pos; int _size = nodeStream.Count;
        
        if (ParseEnd())
        {
            return true;
        }
        pos = _pos;

        if (ParseValue() && ParseEnd())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);
        
        if (ParseVariable() && ParseEqual() && ParseValue() && ParseEnd())
        {
            nodeStream.Add(VMNode.New.VariableNode(valNameStack.Pop()));
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);

        if (ParseBlockIf())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);

        if (ParseBlockWhile())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);

        return false;
    }
    private bool ParseBlockIf()
    {
        int _pos = pos; int _size = nodeStream.Count;

        if (ParseKeywordIf() && ParseBracket("(") && ParseValue() && ParseBracket(")"))
        {
            int tmpIndex = nodeStream.Count;
            nodeStream.Add(null);

            if (ParseBracket("{"))
            {
                while (ParseStatement()) ;
                if (ParseBracket("}"))
                {
                    nodeStream[tmpIndex] = new VMNode(VMNodeType.IFGOTO, nodeStream.Count);
                    return true;
                }
            }
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);

        return false;
    }
    private bool ParseBlockWhile()
    {
        int _pos = pos; int _size = nodeStream.Count;

        int backPos = nodeStream.Count;

        if (ParseKeywordWhile() && ParseBracket("(") && ParseValue() && ParseBracket(")"))
        {
            int tmpIndex = nodeStream.Count;
            nodeStream.Add(null);

            if (ParseBracket("{"))
            {
                while (ParseStatement()) ;
                if (ParseBracket("}"))
                {
                    nodeStream[tmpIndex] = new VMNode(VMNodeType.IFGOTO, nodeStream.Count + 1);
                    nodeStream.Add(new VMNode(VMNodeType.GOTO, backPos));
                    return true;
                }
            }
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);

        return false;
    }
    private bool ParseValue()
    {
        int _pos = pos; int _size = nodeStream.Count;
        
        if (ParseConst())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);
        
        if (ParseFunction2())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);
        
        if (ParseVariableValue())
        {
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);
        
        return false;
    }
    private bool ParseConst()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.NUMBER)
        {
            nodeStream.Add(VMNode.New.ValueNode(float.Parse(token.Val)));
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseEqual()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.SYMBLE && token.Val == "=")
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseVariableValue()
    {
        int _pos = pos;
        if (_ParseVariableName())
        {
            string valName = valNameStack.Pop();
            nodeStream.Add(VMNode.New.NameNode(valName));
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseVariable()
    {
        int _pos = pos;
        if (_ParseVariableName())
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool _ParseVariableName()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.STRING && !functionSet.Contains(token.Val))
        {
            valNameStack.Push(token.Val);
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseFunction2Name()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.STRING && functionSet.Contains(token.Val))
        {
            funcNameStack.Push(token.Val);
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseKeywordIf()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.STRING && token.Val == "if")
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseKeywordWhile()
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.STRING && token.Val == "while")
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseBracket(string bracket)
    {
        int _pos = pos;
        var token = tokenStream[pos++];
        if (token.Type == VMTokenType.BRACKET && token.Val == bracket)
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseComma()
    {
        int _pos = pos;
        if (tokenStream[pos++].Type == VMTokenType.COMMA)
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseEnd()
    {
        int _pos = pos;
        if (tokenStream[pos++].Type == VMTokenType.END)
        {
            return true;
        }
        pos = _pos;
        return false;
    }
    private bool ParseFunction2()
    {
        int _pos = pos; int _size = nodeStream.Count;
        if (ParseFunction2Name() && ParseBracket("(") && ParseValue() && ParseComma() && ParseValue() && ParseBracket(")"))
        {
            nodeStream.Add(VMNode.New.FunctionNode(funcNameStack.Pop()));
            return true;
        }
        pos = _pos; nodeStream.RemoveRange(_size, nodeStream.Count - _size);
        return false;
    }
}

class VMNode
{
    // 静态工厂类
    public static class New
    {
        public static VMNode ValueNode(float value)
        {
            return new VMNode(VMNodeType.VALUE, value);
        }
        public static VMNode VariableNode(string key)
        {
            return new VMNode(VMNodeType.VARIABLE, key);
        }
        public static VMNode NameNode(string str)
        {
            return new VMNode(VMNodeType.NAME, str);
        }
        public static VMNode FunctionNode(string str)
        {
            return new VMNode(VMNodeType.FUNCTION, str);
        }
    };

    // 节点类型，节点关键字，节点值
    private VMNodeType type;
    private object value;

    public VMNode(VMNodeType type, object value)
    {
        this.type = type;
        this.value = value;
    }

    public VMNodeType Type { get => type; }
    public object Value { get => value; }
}

enum VMNodeType : byte
{
    VALUE,
    VARIABLE,
    NAME, FUNCTION,
    IFGOTO, GOTO,
}