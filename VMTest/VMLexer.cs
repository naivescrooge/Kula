using System;
using System.Collections.Generic;
using System.Text;


class VMLexer
{
    private List<VMLexToken> tokenStream;
    private string codes;

    public List<VMLexToken> TokenStream { get => tokenStream ?? Scan(); }

    public VMLexer(string codes)
    {
        this.codes = codes;
    }

    private bool IsNumberChar(char c) { return (c <= '9' && c >= '0') || c == '.' || c == '+' || c == '-'; }
    private bool IsStringChar(char c) 
    {
        return (c <= 'z' && c >= 'a') || (c <= 'Z' && c >= 'A') || (c == '_') || IsNumberChar(c); 
    }
    private bool IsSpaceChar(char c) { return (c == '\n' || c == '\t' || c == '\r' || c == ' '); }
    private bool IsBracket(char c) { return c == '(' || c == '{' || c == ')' || c == '}'; }
    private bool IsComma(char c) { return c == ','; }
    private bool IsEnd(char c) { return c == ';'; }
    private bool IsSpecialChar(char c)
    {
        return !IsNumberChar(c) && !IsStringChar(c) && !IsSpaceChar(c)
            && !IsBracket(c) && !IsComma(c);
    }

    public List<VMLexToken> Scan()
    {
        tokenStream = new List<VMLexToken>();
        VMTokenType? state = null;
        StringBuilder stringBuilder = new StringBuilder();
        for (int p = 0; p < codes.Length; ++p)
        {
            if (state == null)
            {
                char c = codes[p];
                if (IsSpaceChar(c)) { continue; }
                else if (IsNumberChar(c))
                {
                    stringBuilder.Append(c);
                    state = VMTokenType.NUMBER;
                }
                else if (IsStringChar(c))
                {
                    stringBuilder.Append(c);
                    state = VMTokenType.STRING;
                }
                else if (IsComma(c))
                {
                    tokenStream.Add(new VMLexToken(VMTokenType.COMMA, ","));
                }
                else if (IsBracket(c))
                {
                    tokenStream.Add(new VMLexToken(VMTokenType.BRACKET, c.ToString()));
                }
                else if (IsEnd(c))
                {
                    tokenStream.Add(new VMLexToken(VMTokenType.END, ";"));
                }
                else if (IsSpecialChar(c))
                {
                    stringBuilder.Append(c);
                    state = VMTokenType.SYMBLE;
                }
            }
            else
            {
                switch (state)
                {
                    case VMTokenType.STRING:
                        {
                            while (p < codes.Length && IsStringChar(codes[p]))
                            {
                                stringBuilder.Append(codes[p++]);
                            }
                        }
                        break;
                    case VMTokenType.NUMBER:
                        {
                            while (p < codes.Length && IsNumberChar(codes[p]))
                            {
                                stringBuilder.Append(codes[p++]);
                            }

                        }
                        break;
                    case VMTokenType.SYMBLE:
                        {
                            while (p < codes.Length && IsSpecialChar(codes[p]))
                            {
                                stringBuilder.Append(codes[p++]);
                            }
                        }
                        break;
                }
                tokenStream.Add(new VMLexToken((VMTokenType)state, stringBuilder.ToString()));
                state = null;
                stringBuilder.Clear();
                --p;
            }
        }
        if (state != null)
        {
            tokenStream.Add(new VMLexToken((VMTokenType)state, stringBuilder.ToString()));
        }
        return tokenStream;
    }

    public void Show()
    {
        Console.WriteLine("-> Lexer");
        var tmp = tokenStream ?? Scan();
        foreach (VMLexToken token in tmp)
        {
            Console.WriteLine("\t< " + token.Type.ToString() + " | " + token.Val + " >");
        }
        Console.WriteLine();
    }
}
class VMLexToken
{
    VMTokenType type;
    string val;


    public VMLexToken(VMTokenType type, string val)
    {
        this.type = type;
        this.val = val;
    }
    public string Val { get => val; set => val = value; }
    public VMTokenType Type { get => type; set => type = value; }
}

enum VMTokenType : byte
{
    STRING,
    NUMBER,
    SYMBLE,
    COMMA,
    BRACKET,
    END,
}