using DotNetGraph.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compiler
{
    internal class Parser
    {
        static Token t;

        static void match(string s)
        {
            if (s == t.str_lexel)
            {
                t = Scanner.GetToken();
            }
            else
            {
                MessageBox.Show("Syntax Error!!!");
            }
        }

        // program -> stmt-seq
        public static void program()
        {
            t = Scanner.GetToken();
            DotNode temp = stmt_seq();
        }

        // stmt-seq -> stmt {; stmt}
        static DotNode stmt_seq()
        {
            DotNode temp = stmt(), newtemp = null, newtemp2 = null;
            newtemp2 = temp;
            while (t.str_lexel == ";")
            {
                match(";");
                //add node in the same level and match it with the parent of the last node
                newtemp = stmt();
                if (newtemp != null)
                {
                    SyntaxTree.ConnectNodesAtTheSameLevel(newtemp2, newtemp);
                }
                newtemp2 = newtemp;
            }
            return temp;
        }

        // stmt -> if-stmt | repeat-stmt | assign-stmt | read-stmt | write-stmt
        static DotNode stmt()
        {
            DotNode temp = null;
            switch (t.token_type)
            {
                case TokenType.IF:
                    temp = if_stmt(); break;
                case TokenType.REPEAT:
                    temp = repeat_stmt(); break;
                case TokenType.IDENTIFIER:
                    temp = assign_stmt(); break;
                case TokenType.READ:
                    temp = read_stmt(); break;
                case TokenType.WRITE:
                    temp = write_stmt(); break;
            }
            return temp;
        }

        // if-stmt -> if exp then stmt-seq [else stmt-seq] end
        static DotNode if_stmt()
        {
            match("if");
            DotNode temp = SyntaxTree.MakeSquareNode("if");
            SyntaxTree.ConnectChildNodes(temp, exp());
            match("then");
            SyntaxTree.ConnectChildNodes(temp, stmt_seq());
            if (t.str_lexel == "else")
            {
                match("else");
                SyntaxTree.ConnectChildNodes(temp, stmt_seq());
            }
            match("end");
            return temp;
        }

        // repeat-stmt -> repeat stmt-seq until exp
        static DotNode repeat_stmt()
        {
            match("repeat");
            DotNode temp = SyntaxTree.MakeSquareNode("repeat");
            SyntaxTree.ConnectChildNodes(temp, stmt_seq());
            match("until");
            SyntaxTree.ConnectChildNodes(temp, exp());
            return temp;
        }

        // assign-stmt -> identifier := exp
        static DotNode assign_stmt()
        {
            DotNode temp = SyntaxTree.MakeSquareNode("assign\n(" + t.str_lexel + ")");
            match(t.str_lexel);
            match(":=");
            SyntaxTree.ConnectChildNodes(temp, exp());
            return temp;
        }

        // read-stmt -> read identifier
        static DotNode read_stmt()
        {
            match("read");
            DotNode temp = SyntaxTree.MakeSquareNode("read\n(" + t.str_lexel + ")");
            match(t.str_lexel);
            return temp;
        }

        // write-stmt -> write exp
        static DotNode write_stmt()
        {
            DotNode temp;
            match("write");
            temp = SyntaxTree.MakeSquareNode("write");
            SyntaxTree.ConnectChildNodes(temp, exp());
            return temp;
        }

        // exp -> simple-exp [comparison-op simple-exp]
        static DotNode exp()
        {
            DotNode temp = simple_exp();
            if (t.str_lexel == "<" || t.str_lexel == "=")
            {
                DotNode tt = SyntaxTree.MakeOvalNode("op\n(" + t.str_lexel + ")");
                comparison_op();
                SyntaxTree.ConnectChildNodes(tt, temp);
                SyntaxTree.ConnectChildNodes(tt, simple_exp());
                temp = tt;
            }
            return temp;
        }

        // comparison-op -> < | =
        static void comparison_op()
        {
            if (t.str_lexel == "<") match("<");
            if (t.str_lexel == "=") match("=");
        }

        // simple-exp -> term { addop term }
        static DotNode simple_exp()
        {
            DotNode temp, newtemp;
            temp = term();
            while (t.str_lexel == "+" || t.str_lexel == "-")
            {
                newtemp = SyntaxTree.MakeOvalNode("op\n(" + t.str_lexel + ")");
                addop();
                SyntaxTree.ConnectChildNodes(newtemp, temp);
                SyntaxTree.ConnectChildNodes(newtemp, term());
                temp = newtemp;
            }
            return temp;
        }

        // addop -> + | -
        static void addop()
        {
            if (t.str_lexel == "+") match("+");
            if (t.str_lexel == "-") match("-");
        }

        // term -> factor { mulop factor }
        static DotNode term()
        {
            DotNode temp, newtemp;
            temp = factor();
            while (t.str_lexel == "*" || t.str_lexel == "/")
            {
                newtemp = SyntaxTree.MakeOvalNode("op\n(" + t.str_lexel + ")");
                mulop();
                SyntaxTree.ConnectChildNodes(newtemp, temp);
                SyntaxTree.ConnectChildNodes(newtemp, factor());
                temp = newtemp;
            }
            return temp;
        }

        // mulop -> * | /
        static void mulop()
        {
            if (t.str_lexel == "*") match("*");
            if (t.str_lexel == "/") match("/");
        }

        // factor -> (exp) | number | identifier
        static DotNode factor()
        {
            DotNode temp = null;
            if (t.str_lexel == "(")
            {
                match("(");
                temp = exp();
                match(")");
            }
            else if (t.token_type == TokenType.NUMBER)
            {
                temp = SyntaxTree.MakeOvalNode("const\n(" + t.num_lexel.ToString() + ")");
                match(t.str_lexel);
            }
            else if (t.token_type == TokenType.IDENTIFIER)
            {
                temp = SyntaxTree.MakeOvalNode("id\n(" + t.str_lexel + ")");
                match(t.str_lexel);
            }
            return temp;
        }
        public static void switchFunction(string s, string str)
        {
            switch (str)
            {
                case "SEMICOLON":
                    {
                        t.token_type = TokenType.SEMICOLON;
                        t.str_lexel = ";";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "ELSE":
                    {
                        t.token_type = TokenType.ELSE;
                        t.str_lexel = "else";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "IF":
                    {
                        t.token_type = TokenType.IF;
                        t.str_lexel = "if";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "THEN":
                    {
                        t.token_type = TokenType.THEN;
                        t.str_lexel = "then";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "END":
                    {
                        t.token_type = TokenType.END;
                        t.str_lexel = "end";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "REPEAT":
                    {
                        t.token_type = TokenType.REPEAT;
                        t.str_lexel = "repeat";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "UNTIL":
                    {
                        t.token_type = TokenType.UNTIL;
                        t.str_lexel = "until";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "IDENTIFIER":
                    {
                        t.token_type = TokenType.IDENTIFIER;
                        t.str_lexel = s;
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "ASSIGN":
                    {
                        t.token_type = TokenType.ASSIGN;
                        t.str_lexel = ":=";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "READ":
                    {
                        t.token_type = TokenType.READ;
                        t.str_lexel = "read";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "WRITE":
                    {
                        t.token_type = TokenType.WRITE;
                        t.str_lexel = "write";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "LESSTHAN":
                    {
                        t.token_type = TokenType.LESSTHAN;
                        t.str_lexel = "<";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "EQUAL":
                    {
                        t.token_type = TokenType.EQUAL;
                        t.str_lexel = "=";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "PLUS":
                    {
                        t.token_type = TokenType.PLUS;
                        t.str_lexel = "+";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "MINUS":
                    {
                        t.token_type = TokenType.MINUS;
                        t.str_lexel = "-";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "MULT":
                    {
                        t.token_type = TokenType.MULT;
                        t.str_lexel = "*";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "DIV":
                    {
                        t.token_type = TokenType.DIV;
                        t.str_lexel = "/";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "OPENBRACKET":
                    {
                        t.token_type = TokenType.OPENBRACKET;
                        t.str_lexel = "(";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "CLOSEDBRACKET":
                    {
                        t.token_type = TokenType.CLOSEDBRACKET;
                        t.str_lexel = ")";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
                case "NUMBER":
                    {
                        t.token_type = TokenType.NUMBER;
                        t.str_lexel = "";
                        t.num_lexel = Int32.Parse(s);
                        Scanner.output.Add(t);
                        break;
                    }
                case "ERROR":
                    {
                        t.token_type = TokenType.ERROR;
                        t.str_lexel = "Syntax Error";
                        t.num_lexel = 0;
                        Scanner.output.Add(t);
                        break;
                    }
            }
        }
        public static void parse(string input)
        {
            Scanner.output.Clear();
            string[] tokens = input.Split('\n');
            string[] splited_token;
            string s = "", str = "";
            Token t;
            int i = 0;
            while (i < tokens.Length)
            {
                splited_token = tokens[i].Split(',');
                s = splited_token[0];
                if (splited_token.Length == 2)
                {
                    str = splited_token[1].Trim();
                    switchFunction(s, str);
                }
                i++;
            }
            if (Scanner.output.Count == 0)
            {
                MessageBox.Show("Empty Input Tokens");
                Application.Restart();
            }
            program();
            SyntaxTree.GenerateSyntaxTree();
            Scanner.output.Clear();
        }
    }
}