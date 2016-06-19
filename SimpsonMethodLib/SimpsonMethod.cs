using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FormulaExecutorLib;

namespace SimpsonMethodLib
{
    public class SimpsonMethod : Component
    {
        public double Integration(FormulaExecutor integrand, double a, double b, double eps)
        {
            //заменим особые предельные точки на точки из окрестности
            if(Double.IsNaN(integrand.Execute(a))||Double.IsInfinity(integrand.Execute(a)))
                a += 0.0001*eps*(b-a);
            if(Double.IsNaN(integrand.Execute(b))||Double.IsInfinity(integrand.Execute(b)))
                b -= 0.0001*eps*(b-a);
            //последовательно посчитаем производные до 4 порядка на 100 точках
            int n = 1000;
            //первая производная
            double[] deriv = new double[n];
            deriv[0] = (integrand.Execute(a+(b-a)/(n-1)) - integrand.Execute(a)) / ((b-a)/(n-1));
            for (int i = 1; i < n; ++i)
            {
                deriv[i] = (integrand.Execute(a+i*(b-a)/(n-1)) - integrand.Execute(a+(i-1)*(b-a)/(n-1))) / ((b-a)/(n-1));
            }
            //вторая производная
            double[] secDeriv = new double[n];
            secDeriv[0] = (deriv[1] - deriv[0]) / ((b-a)/(n-1));
            for (int i = 1; i < n; ++i)
            {
                secDeriv[i] = (deriv[i] - deriv[i-1]) / ((b-a)/(n-1));
            }
            //третья производная
            double[] thirdDeriv = new double[n];
            thirdDeriv[0] = (secDeriv[1] - secDeriv[0]) / ((b-a)/(n-1));
            for (int i = 1; i < n; ++i)
            {
                thirdDeriv[i] = (secDeriv[i] - secDeriv[i - 1]) / ((b-a)/(n-1));
            }
            //четвертая производная
            double[] fourthDeriv = new double[n];
            fourthDeriv[0] = (thirdDeriv[1] - thirdDeriv[0]) / ((b-a)/(n-1));
            double maxFourthDeriv = Math.Abs(fourthDeriv[0]);
            for (int i = 1; i < n; ++i)
            {
                fourthDeriv[i] = (thirdDeriv[i] - thirdDeriv[i - 1]) / ((b-a)/(n-1));
                if (Math.Abs(fourthDeriv[i]) > maxFourthDeriv) maxFourthDeriv = Math.Abs(fourthDeriv[i]);
            }
            //определим шаг разбиения h для метода
            double h = (b-a)/Math.Ceiling(Math.Pow(maxFourthDeriv*Math.Pow(b-a, 5)/(2880*eps), 0.25));
            //найдем значение интеграла
            double sum = 0;
            for (int i = 0; i * h < (b - a); ++i)
                sum += h/6*(integrand.Execute(a+i*h) + 4*integrand.Execute(a+i*h+h/2)+ integrand.Execute(a+(i+1)*h));
            return sum;
        }
    }
}
