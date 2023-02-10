using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Compiler
{
    public enum TokenType
    {
        SEMICOLON,
        IF,
        THEN,
        END,
        REPEAT,
        UNTIL,
        IDENTIFIER,
        ASSIGN,
        READ,
        WRITE,
        LESSTHAN,
        EQUAL,
        PLUS,
        MINUS,
        MULT,
        DIV,
        OPENBRACKET,
        CLOSEDBRACKET,
        NUMBER,
        ELSE,
        ERROR
    };

    public struct Token
    {
        public TokenType token_type;
        public string str_lexel;
        public int num_lexel;
    }

    internal class Scanner
    {
        public static List<Token> output = new List<Token>();
        static int list_cnt = 0;

        public static void scan(string input)
        {
            string s = "";
            Token t;
            int i = 0;
            input += '\n';
            while (i < input.Length)
            {
                if ((input[i] >= 'A' && input[i] <= 'Z') || (input[i] >= 'a' && input[i] <= 'z'))
                {
                    while ((input[i] >= 'A' && input[i] <= 'Z') || (input[i] >= 'a' && input[i] <= 'z'))
                    {
                        s += input[i++];
                    }
                    if (input[i] >= '0' && input[i] <= '9')
                    {
                        t.token_type = TokenType.ERROR;
                        t.str_lexel = "Syntax Error";
                        t.num_lexel = 0;
                        output.Add(t);
                        MessageBox.Show("Syntax Error!!!");
                        break;
                    }
                    if (s == "if")
                    {
                        t.token_type = TokenType.IF;
                        t.str_lexel = "if";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "then")
                    {
                        t.token_type = TokenType.THEN;
                        t.str_lexel = "then";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "end")
                    {
                        t.token_type = TokenType.END;
                        t.str_lexel = "end";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "else")
                    {
                        t.token_type = TokenType.ELSE;
                        t.str_lexel = "else";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "repeat")
                    {
                        t.token_type = TokenType.REPEAT;
                        t.str_lexel = "repeat";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "until")
                    {
                        t.token_type = TokenType.UNTIL;
                        t.str_lexel = "until";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "read")
                    {
                        t.token_type = TokenType.READ;
                        t.str_lexel = "read";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else if (s == "write")
                    {
                        t.token_type = TokenType.WRITE;
                        t.str_lexel = "write";
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    else
                    {
                        t.token_type = TokenType.IDENTIFIER;
                        t.str_lexel = s;
                        t.num_lexel = 0;
                        output.Add(t);
                    }
                    s = "";
                }
                else if (input[i] >= '0' && input[i] <= '9')
                {
                    while (input[i] >= '0' && input[i] <= '9')
                    {
                        s += input[i++];
                    }
                    t.token_type = TokenType.NUMBER;
                    t.str_lexel = "";
                    t.num_lexel = Int32.Parse(s);
                    output.Add(t);
                    s = "";
                }
                else if (input[i] == ' ' || input[i] == '\t' || input[i] == '\r' || input[i] == '\n')
                {
                    i++;
                    continue;
                }
                else if (input[i] == ';')
                {
                    t.token_type = TokenType.SEMICOLON;
                    t.str_lexel = ";";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '<')
                {
                    t.token_type = TokenType.LESSTHAN;
                    t.str_lexel = "<";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '=')
                {
                    t.token_type = TokenType.EQUAL;
                    t.str_lexel = "=";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '+')
                {
                    t.token_type = TokenType.PLUS;
                    t.str_lexel = "+";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '-')
                {
                    t.token_type = TokenType.MINUS;
                    t.str_lexel = "-";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '*')
                {
                    t.token_type = TokenType.MULT;
                    t.str_lexel = "*";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '/')
                {
                    t.token_type = TokenType.DIV;
                    t.str_lexel = "/";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == '(')
                {
                    t.token_type = TokenType.OPENBRACKET;
                    t.str_lexel = "(";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == ')')
                {
                    t.token_type = TokenType.CLOSEDBRACKET;
                    t.str_lexel = ")";
                    t.num_lexel = 0;
                    output.Add(t);
                    i++;
                }
                else if (input[i] == ':' && input[i + 1] == '=')
                {
                    t.token_type = TokenType.ASSIGN;
                    t.str_lexel = ":=";
                    t.num_lexel = 0;
                    output.Add(t);
                    i += 2;
                }
                else if (input[i] == '{')
                {
                    i++;
                    while (input[i++] != '}') ;
                }
                else
                {
                    t.token_type = TokenType.ERROR;
                    t.str_lexel = "Syntax Error";
                    t.num_lexel = 0;
                    output.Add(t);
                    MessageBox.Show("Syntax Error!!!");
                    break;
                }
            }
            if(output.Count == 0)
            {
                MessageBox.Show("Empty Input Code");
                Application.Restart();
            }
        }
        public static Token GetToken()
        {
            if (list_cnt < output.Count)
            {
                return output[list_cnt++];
            }
            Token t;
            t.token_type = TokenType.SEMICOLON;
            t.str_lexel = "done";
            t.num_lexel = 555;
            list_cnt = 0;
            output.Clear();
            return t;
        }
    }
}
