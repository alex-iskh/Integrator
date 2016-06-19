using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ParserLib;
using FormulaExecutorLib;
using RectangleMethodLib;
using TrapezoidalMethodLib;
using SimpsonMethodLib;

namespace IntegratorClient
{
    public partial class Integrator : Form
    {
        public Integrator()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked) radioButton1.Checked = false;
            else radioButton1.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            Parser parserComp = new Parser();
            //при помощи компонента-парсера преобразуем входные данные
            //в формат, используемый другими компонентами:
            //считаем пределы интегрирования и погрешность
            double a = 0, b = 0, eps = 0;
            if (!parserComp.ParseParam(ref a, ref b, ref eps, textBox2.Text, textBox3.Text, textBox4.Text))
            {
                richTextBox1.Text += "Ошибка ввода параметров";
                return;
            }
            parserComp.Dispose();
            //преобразуем строку в очередь токенов в обратной польской нотации
            //для компонента, вычисляющего значения формулы при подстановке
            FormulaExecutor formulaComp = new FormulaExecutor(parserComp.ShuntingYard(textBox1.Text));
            if (formulaComp.formula == null)
            {
                richTextBox1.Text += "Ошибка ввода выражения";
                return;
            }
            //при помощи компонентов методов численного интегрирования вычислим значение опред. интеграла
            Container methodComponents = new Container();
            if (checkBox1.Checked)
            {
                RectangleMethod rectangleComp = new RectangleMethod();
                methodComponents.Add(rectangleComp);
                richTextBox1.Text += (rectangleComp.Integration(formulaComp, a, b, eps).ToString() 
                    + " (метод левых прямоугольников)\n");
            }
            if (checkBox2.Checked)
            {
                TrapezoidalMethod trapezoidComp = new TrapezoidalMethod();
                methodComponents.Add(trapezoidComp);
                richTextBox1.Text += (trapezoidComp.Integration(formulaComp, a, b, eps).ToString()
                    + " (метод трапеций)\n");
            }
            if (checkBox3.Checked)
            {
                SimpsonMethod simpsonComp = new SimpsonMethod();
                methodComponents.Add(simpsonComp);
                richTextBox1.Text += (simpsonComp.Integration(formulaComp, a, b, eps).ToString()
                    + " (метод Симпсона)\n");
            }
            methodComponents.Dispose();
            formulaComp.Dispose();
        }
    }
}
