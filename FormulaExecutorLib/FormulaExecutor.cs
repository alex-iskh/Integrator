using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FormulaExecutorLib
{
    public class FormulaExecutor : Component
    {
        public FormulaExecutor(Queue<string> rpn)
        {
            formula = rpn;
        }

        public Queue<string> formula;

        public double Execute(double x)
        {
            Queue<string> expr = new Queue<string>(formula);
            //вычислим выражение, записанное в формате обратной польской нотации
            //с помощью стека, в котором будем хранить операнды
            Stack<double> operandStack = new Stack<double>();
            string token;
            double operand1, operand2;
            while (expr.Count > 0)
            {
                token = expr.Dequeue();
                switch (token)
                {
                    case "+":
                        if (operandStack.Count < 2) return Double.NaN;
                        operand2 = operandStack.Pop();
                        operand1 = operandStack.Pop();
                        operandStack.Push(operand1+operand2);
                        break;
                    case "-":
                        if (operandStack.Count < 2) return Double.NaN;
                        operand2 = operandStack.Pop();
                        operand1 = operandStack.Pop();
                        operandStack.Push(operand1-operand2);
                        break;
                    case "*":
                        if (operandStack.Count < 2) return Double.NaN;
                        operand2 = operandStack.Pop();
                        operand1 = operandStack.Pop();
                        operandStack.Push(operand1*operand2);
                        break;
                    case "/":
                        if (operandStack.Count < 2) return Double.NaN;
                        operand2 = operandStack.Pop();
                        operand1 = operandStack.Pop();
                        operandStack.Push(operand1/operand2);
                        break;
                    case "^":
                        if (operandStack.Count < 2) return Double.NaN;
                        operand2 = operandStack.Pop();
                        operand1 = operandStack.Pop();
                        operandStack.Push(Math.Pow(operand1, operand2));
                        break;
                    case "ln":
                        if (operandStack.Count < 1) return Double.NaN;
                        operand1 = operandStack.Pop();
                        operandStack.Push(Math.Log(operand1));
                        break;
                    case "sqrt":
                        if (operandStack.Count < 1) return Double.NaN;
                        operand1 = operandStack.Pop();
                        operandStack.Push(Math.Pow(operand1, 0.5));
                        break;
                    case "x":
                        operandStack.Push(x);
                        break;
                    default:
                        operandStack.Push(Double.Parse(token));
                        break;
                }
            }
            return operandStack.Pop();
        }
    }
}
