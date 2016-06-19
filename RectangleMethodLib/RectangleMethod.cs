using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FormulaExecutorLib;

namespace RectangleMethodLib
{
    public class RectangleMethod : Component
    {
        public double Integration(FormulaExecutor integrand, double a, double b, double eps)
        {
            //заменим особые предельные точки на точки из окрестности
            if(Double.IsNaN(integrand.Execute(a))||Double.IsInfinity(integrand.Execute(a)))
                a += 0.0001*eps*(b-a);
            if(Double.IsNaN(integrand.Execute(b))||Double.IsInfinity(integrand.Execute(b)))
                b -= 0.0001*eps*(b-a);
            //посчитаем производную на 100 точках и определим шаг разбиения h для метода
            int n = 100;
            double[] deriv = new double[n];
            deriv[0] = (integrand.Execute(a+(b-a)/(n-1)) - integrand.Execute(a)) / ((b-a)/(n-1));
            double maxDeriv = Math.Abs(deriv[0]);
            for (int i = 1; i < n; ++i)
            {
                deriv[i] = (integrand.Execute(a+i*(b-a)/(n-1)) - integrand.Execute(a+(i-1)*(b-a)/(n-1))) / ((b-a)/(n-1));
                if (Math.Abs(deriv[i]) > maxDeriv) maxDeriv = Math.Abs(deriv[i]);
            }
            double h = (b-a)/Math.Ceiling(0.5*maxDeriv*(b-a)*(b-a)/eps);
            //найдем значение интеграла
            double sum = 0;
            for (int i = 0; i * h < (b - a); ++i)
                sum += integrand.Execute(a+i*h);
            return sum*h;
        }
    }
}
