using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace ParserLib
{
    public class Parser : Component
    {
       public Queue<string> ShuntingYard(string input)
        {
            if (input == null) return null;
            //составим очередь из токенов в порядке входной строки, выделяя токены при помощи регулярного выражения
            Queue<string> infix = new Queue<string>();
            string token;
            Regex rgx = new Regex(@"[0-9]+(?:,[0-9]+)?|[\+\-\*\/\^)(]|ln|sqrt|x");
            while (!String.IsNullOrWhiteSpace(input))
            {
                input = input.Trim();
                if (rgx.IsMatch(input, 0))
                {
                    token = rgx.Match(input).ToString();
                    input = input.Remove(0, token.Length);
                    //заменим все выражения с унарным минусом вида "-a" выражением "0-a"
                    if ((token == "-")&&((infix.Count == 0)||(infix.Last() == "(")))
                    {
                        infix.Enqueue("0");
                    }
                    infix.Enqueue(token);
                }
                else return null;
            }
            //преобразуем порядок токенов в формат обратной польской записи
            //используя классический алгоритм сортировочной станции
            Queue<string> rpn = new Queue<string>();
            Stack<string> opStack = new Stack<string>();
            while(infix.Count > 0)
            {
                token = infix.Dequeue();
                if ((token[0] == '0')||(token[0] == '1')||(token[0] == '2')||(token[0] == '3')||
                    (token[0] == '4')||(token[0] == '5')||(token[0] == '6')||(token[0] == '7')||
                    (token[0] == '8')||(token[0] == '9')||(token[0] == 'x'))
                {
                    rpn.Enqueue(token);
                }
                else {
                    if ((token == "ln") || (token == "sqrt"))
                    {
                        opStack.Push(token);
                    }
                    else {
                        if ((token == "+") || (token == "-") || (token == "*") || (token == "/"))
                        {
                            while ((opStack.Count>0)&&(Precedence(token)<=Precedence(opStack.Peek())))
                                rpn.Enqueue(opStack.Pop());
                            opStack.Push(token);
                        }
                        else {
                            if (token == "^")
                            {
                                while ((opStack.Count>0)&&(Precedence(token)<Precedence(opStack.Peek())))
                                    rpn.Enqueue(opStack.Pop());
                                opStack.Push(token);
                            }
                            else {
                                if (token == "(")
                                    opStack.Push(token);
                                else {
                                    if (token == ")")
                                    {
                                        while (opStack.Peek() != "(")
                                        {
                                            rpn.Enqueue(opStack.Pop());
                                            if (opStack.Count == 0) return null;
                                        }
                                        opStack.Pop();
                                        if ((opStack.Count>0)&&((opStack.Peek() == "ln")||(opStack.Peek() == "sqrt")))
                                            rpn.Enqueue(opStack.Pop());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            while (opStack.Count > 0)
            {
                if (opStack.Peek() == "(") return null;
                rpn.Enqueue(opStack.Pop());
            }
            return rpn;
        }

        int Precedence(string operand)
        {
            switch (operand)
            {
                case "+":
                    return 1;
                case "-":
                    return 1;
                case "*":
                    return 2;
                case "/":
                    return 2;
                case "^":
                    return 3;
                default:
                    return 0;
            }
        }

        public bool ParseParam(ref double a, ref double b, ref double eps, string strA, string strB, string strEps)
        {
            try {
                a = Double.Parse(strA);
                b = Double.Parse(strB);
                eps = Double.Parse(strEps);
            }
            catch {
                //в случае некорректного ввода параметра этот метод возвращает false
                return false;
            }
            if (a > b) return false;
            return true;
        }
    }
}
